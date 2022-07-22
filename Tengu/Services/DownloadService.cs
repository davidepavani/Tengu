using Avalonia.Collections;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Models;
using NLog;
using Tengu.Interfaces;
using Tengu.Data;
using Tengu.Business.Commons.Models;
using Tengu.Business.Commons.Objects;
using Tengu.Business.API.Interfaces;
using System.Threading;
using Downla.Models;

namespace Tengu.Services
{
    public class DownloadService : ReactiveObject, IDownloadService
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.MainLogger);
        private readonly object downloadSync = new();

        private static ITenguApi TenguApi => Locator.Current.GetService<ITenguApi>();

        private AvaloniaList<HistoryModel> historyList = new();
        private AvaloniaList<DownloadModel> animeQueue = new();
        private AvaloniaList<DownloadModel> currentDownloads = new();

        private int downloadCount = 0;

        #region Properties
        public AvaloniaList<DownloadModel> CurrentDownloads 
        { 
            get => currentDownloads;
            private set => this.RaiseAndSetIfChanged(ref currentDownloads, value);
        }
        public AvaloniaList<DownloadModel> AnimeQueue
        {
            get => animeQueue;
            private set => this.RaiseAndSetIfChanged(ref animeQueue, value);
        }
        public AvaloniaList<HistoryModel> HistoryList
        {
            get => historyList;
            private set => this.RaiseAndSetIfChanged(ref historyList, value);
        }
        public int DownloadCount
        {
            get => downloadCount;
            private set => this.RaiseAndSetIfChanged(ref downloadCount, value);
        }
        #endregion

        public void EnqueueAnime(EpisodeModel episode)
        {
            lock (downloadSync)
            {
                if (AnimeAlreadyInQueue(episode.Title, episode.EpisodeNumber, episode.Host))
                {
                    log.Warn("[{host}] Already in queued: {Title} | Episode {EpisodeNumber}", episode.Host, episode.Title, episode.EpisodeNumber);
                    return;
                }

                AnimeQueue.Add(new(episode));
                RefreshDownloadCount();

                log.Info("[{host}] Enqueued {Title} | Episode {EpisodeNumber}", episode.Host, episode.Title, episode.EpisodeNumber);

                if (!CurrentDownloads.Any(x => x.Episode.Host == TenguHosts.AnimeSaturn) && 
                    AnimeQueue.Any(x => x.Episode.Host == TenguHosts.AnimeSaturn))
                {
                    Task.Run(() => ExecuteDownload(TenguHosts.AnimeSaturn));
                }

                if (!CurrentDownloads.Any(x => x.Episode.Host == TenguHosts.AnimeUnity) && 
                    AnimeQueue.Any(x => x.Episode.Host == TenguHosts.AnimeUnity))
                {
                    Task.Run(() => ExecuteDownload(TenguHosts.AnimeUnity));
                }
            }
        }

        public void CancelDownload(DownloadModel download)
        {
            if (download != null)
            {
                lock (downloadSync)
                {
                    if (AnimeQueue.Any(x => x.Episode.Title.Equals(download.Episode.Title, StringComparison.CurrentCultureIgnoreCase) &&
                                           x.Episode.EpisodeNumber.Equals(download.Episode.EpisodeNumber, StringComparison.CurrentCultureIgnoreCase) &&
                                           x.Episode.Host.Equals(download.Episode.Host)))
                    {
                        AnimeQueue.Remove(download);
                        AddToHistory(download, "Aborted by the user");
                    }
                    else if (CurrentDownloads.Any(x => x.Episode.Title.Equals(download.Episode.Title, StringComparison.CurrentCultureIgnoreCase) &&
                                                       x.Episode.EpisodeNumber.Equals(download.Episode.EpisodeNumber, StringComparison.CurrentCultureIgnoreCase) &&
                                                       x.Episode.Host.Equals(download.Episode.Host)))
                    {
                        download.TokenSource.Cancel();
                    }

                    RefreshDownloadCount();
                }
            }
        }

        private void ExecuteDownload(TenguHosts host)
        {
            DownloadModel download = GetNextEpisodeByHost(host);

            log.Info("[{host}] Initializing Download: {title} | Episode {EpisodeNumber}", host, download.Episode.Title, download.Episode.EpisodeNumber);

            while (download != null)
            {
                AsyncDownload(download).Wait();

                RefreshDownloadCount();

                // Load Next
                download = GetNextEpisodeByHost(host);
            }

            RefreshDownloadCount();
        }

        private async Task AsyncDownload(DownloadModel download)
        {
            string errors = string.Empty;
            TenguResult<DownloadMonitor> result = null;

            lock (downloadSync)
            {
                // Remove it from QUEUE
                AnimeQueue.Remove(download);
                CurrentDownloads.Add(download);
            }

            try
            {
                download.TokenSource = new();

                result = await TenguApi.StartDownloadAsync(download.Episode.DownloadUrl, download.Episode.Host, download.TokenSource.Token);
                download.DownloadInfo = result.Data;

                await download.DownloadInfo.EnsureDownloadCompletion();
            }
            catch (Exception ex)
            {
                errors += ex.Message + "\n";
                errors += "-------------------------\n";

                log.Fatal(ex, "[{host}] Download Exception: {title} | Episode {EpisodeNumber}", download.Episode.Host, download.Episode.Title, download.Episode.EpisodeNumber);
            }
            finally
            {
                // todo => check exceptions bla bla
                foreach (TenguResultInfo infoRes in result.Infos)
                {
                    if (!infoRes.Success)
                    {
                        errors += infoRes.Exception.Message + "\n";
                        errors += "-------------------------\n";

                        log.Debug(infoRes.Exception, "[{host}] TenguResultInfo Exception: {title} | Episode {EpisodeNumber} | Host: {host}", infoRes.Host, download.Episode.Title, download.Episode.EpisodeNumber);
                    }
                }

                download.TokenSource.Dispose();
                download.TokenSource = null;

                lock (downloadSync)
                {
                    CurrentDownloads.Remove(download);
                    AddToHistory(download, errors);
                }
            }
        }

        private void RefreshDownloadCount()
        {
            lock (downloadSync)
            {
                DownloadCount = AnimeQueue.Count;
                DownloadCount += CurrentDownloads.Count;
            }
        }

        private void AddToHistory(DownloadModel download, string errors)
        {
            HistoryList.Add(new()
            {
                Name = download.Episode.Title,
                Episode = download.Episode.EpisodeNumber,
                ErrorMessage = errors,
                Host = download.Episode.Host,
                InError = download.DownloadInfo == null ? true : download.DownloadInfo.Status == Downla.DownloadStatuses.Faulted ||
                                                                 download.DownloadInfo.Status == Downla.DownloadStatuses.Canceled,
                EndTime = DateTime.Now
            });
        }

        private DownloadModel GetNextEpisodeByHost(TenguHosts host)
            => AnimeQueue.FirstOrDefault(x => x.Episode.Host.Equals(host), null);

        // Masterpiece
        private bool AnimeAlreadyInQueue(string title, string ep, TenguHosts host)
            => AnimeQueue.Any(x => x.Episode.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && x.Episode.EpisodeNumber.Equals(ep, StringComparison.CurrentCultureIgnoreCase) && x.Episode.Host.Equals(host)) ||
               CurrentDownloads.Any(x => x.Episode.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && x.Episode.EpisodeNumber.Equals(ep, StringComparison.CurrentCultureIgnoreCase) && x.Episode.Host.Equals(host));
    }
}
