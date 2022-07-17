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

        private static ITenguApi TenguApi => Locator.Current.GetService<ITenguApi>();

        private CancellationTokenSource saturnTokenSource;
        private CancellationTokenSource unityTokenSource;

        private AvaloniaList<HistoryModel> historyList = new();
        private AvaloniaList<DownloadModel> animeQueue = new();
        
        private int downloadCount = 0;

        private bool downloadingSaturn = false;
        private bool downloadingUnity = false;

        private string saturnStatusMessage = string.Empty;

        private DownloadModel currentSaturnDownload = null;
        private DownloadModel currentUnityDownload = null;

        #region Properties
        public DownloadModel CurrentUnityDownload 
        { 
            get => currentUnityDownload; 
            set => this.RaiseAndSetIfChanged(ref currentUnityDownload, value);
        }
        public DownloadModel CurrentSaturnDownload 
        { 
            get => currentSaturnDownload; 
            set => this.RaiseAndSetIfChanged(ref currentSaturnDownload, value);
        }
        public AvaloniaList<DownloadModel> AnimeQueue
        {
            get => animeQueue;
            set => this.RaiseAndSetIfChanged(ref animeQueue, value);
        }
        public AvaloniaList<HistoryModel> HistoryList
        {
            get => historyList;
            set => this.RaiseAndSetIfChanged(ref historyList, value);
        }
        public int DownloadCount
        {
            get => downloadCount;
            set => this.RaiseAndSetIfChanged(ref downloadCount, value);
        }
        #endregion

        public void EnqueueAnime(EpisodeModel episode)
        {
            if(AnimeAlreadyInQueue(episode.Title, episode.EpisodeNumber, episode.Host))
            {
                log.Warn("[{host}] Already in queued: {Title} | Episode {EpisodeNumber}", episode.Host, episode.Title, episode.EpisodeNumber);
                return;
            }
            
            AnimeQueue.Add(new(episode));
            RefreshDownloadCount();

            log.Info("[{host}] Enqueued {Title} | Episode {EpisodeNumber}", episode.Host, episode.Title, episode.EpisodeNumber);

            if (!downloadingSaturn && AnimeQueue.Any(x => x.Episode.Host == TenguHosts.AnimeSaturn))
            {
                Task.Run(() => SaturnDownloadAsync());
            }

            if (!downloadingUnity && AnimeQueue.Any(x => x.Episode.Host == TenguHosts.AnimeUnity))
            {
                Task.Run(() => UnityDownload());
            }
        }

        private void UnityDownload()
        {

        }
        private async void SaturnDownloadAsync()
        {
            CurrentSaturnDownload = GetNextEpisodeByHost(TenguHosts.AnimeSaturn);
            downloadingSaturn = true;

            log.Info("[Saturn] Initializing Download: {title} | Episode {EpisodeNumber}", CurrentSaturnDownload.Episode.Title, CurrentSaturnDownload.Episode.EpisodeNumber);

            while (CurrentSaturnDownload != null)
            {
                saturnTokenSource = new();
                saturnStatusMessage = string.Empty;

                // Remove it from QUEUE
                AnimeQueue.Remove(CurrentSaturnDownload);

                try
                {
                    TenguResult<DownloadMonitor> result = await TenguApi.StartDownloadAsync(CurrentSaturnDownload.Episode.DownloadUrl, CurrentSaturnDownload.Episode.Host, saturnTokenSource.Token);

                    result.Data.OnStatusChange += Data_OnStatusChange;
                    await result.Data.EnsureDownload();

                    CurrentSaturnDownload.DownloadInfo = result.Data;
                }
                catch(Exception ex)
                {
                    saturnStatusMessage += ex.Message + "\n";
                    saturnStatusMessage += "-------------------------\n";

                    log.Fatal(ex, "[Saturn] Download Exception: {title} | Episode {EpisodeNumber}", CurrentSaturnDownload.Episode.Title, CurrentSaturnDownload.Episode.EpisodeNumber);
                }
                finally
                {
                    HistoryList.Add(new()
                    {
                        Name = CurrentSaturnDownload.Episode.Title,
                        Episode = CurrentSaturnDownload.Episode.EpisodeNumber,
                        ErrorMessage = saturnStatusMessage,
                        Host = TenguHosts.AnimeSaturn,
                        InError = CurrentSaturnDownload.DownloadInfo.Status == Downla.DownloadStatuses.Faulted ||
                                  CurrentSaturnDownload.DownloadInfo.Status == Downla.DownloadStatuses.Canceled,
                        EndTime = DateTime.Now
                    });

                    saturnTokenSource.Dispose();
                    saturnTokenSource = null;

                    // Load Next
                    CurrentSaturnDownload = GetNextEpisodeByHost(TenguHosts.AnimeSaturn);
                }
            }

            downloadingSaturn = false;
            RefreshDownloadCount();
        }

        private void Data_OnStatusChange(Downla.DownloadStatuses status, DownloadMonitorInfos infos, IEnumerable<Exception> exceptions)
        {
            // Check Status
            //foreach (TenguResultInfo info in result.Infos)
            //{
            //    if (!info.Success)
            //    {
            //        statusMessage += info.Exception.Message + "\n";
            //        statusMessage += "-------------------------\n";

            //        log.Warn(info.Exception, "[Saturn] Download Info Status: {title} | Episode {EpisodeNumber}", CurrentSaturnDownload.Episode.Title, CurrentSaturnDownload.Episode.EpisodeNumber);
            //    }
            //}
        }

        private void RefreshDownloadCount()
        {
            DownloadCount = AnimeQueue.Count;

            if (CurrentSaturnDownload != null) DownloadCount++;
            if (CurrentUnityDownload != null) DownloadCount++;
        }

        private DownloadModel GetNextEpisodeByHost(TenguHosts host)
            => AnimeQueue.FirstOrDefault(x => x.Episode.Host.Equals(host), null);

        // Masterpiece
        private bool AnimeAlreadyInQueue(string title, string ep, TenguHosts host)
            => AnimeQueue.Any(x => x.Episode.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && x.Episode.EpisodeNumber.Equals(ep, StringComparison.CurrentCultureIgnoreCase) && x.Episode.Host.Equals(host)) ||
                (CurrentSaturnDownload != null && CurrentSaturnDownload.Episode.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && CurrentSaturnDownload.Episode.EpisodeNumber.Equals(ep, StringComparison.CurrentCultureIgnoreCase) ||
                (CurrentUnityDownload != null && CurrentUnityDownload.Episode.Title.Equals(title, StringComparison.CurrentCultureIgnoreCase) && CurrentUnityDownload.Episode.EpisodeNumber.Equals(ep, StringComparison.CurrentCultureIgnoreCase)));
    }
}
