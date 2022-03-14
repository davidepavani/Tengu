using Avalonia.Collections;
using Avalonia.Threading;
using Downla;
using NLog;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Interfaces;
using Tengu.Logging;
using Tengu.Models;
using Tengu.Utilities;

namespace Tengu.Downloads
{
    public class DownloadManager : ReactiveObject, IDownloadManager
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.DownloadLoggerName);

        private static ITenguApi TenguApi => Locator.Current.GetService<ITenguApi>();

        private AvaloniaList<DownloadModel> queueAnimeSaturn = new();
        private AvaloniaList<DownloadModel> queueAnimeUnity = new();

        private int saturnDownloadCount = 0;
        private int unityDownloadCount = 0;

        private bool saturnDownloading = false;
        private bool unityDownloading = false;

        #region Properties
        public AvaloniaList<DownloadModel> QueueAnimeSaturn
        {
            get => queueAnimeSaturn;
            set
            {
                this.RaiseAndSetIfChanged(ref queueAnimeSaturn, value);
                SaturnDownloadCount = QueueAnimeSaturn.Count;
            }
        }
        public AvaloniaList<DownloadModel> QueueAnimeUnity
        {
            get => queueAnimeUnity;
            set
            {
                this.RaiseAndSetIfChanged(ref queueAnimeUnity, value);
                UnityDownloadCount = QueueAnimeUnity.Count;
            }
        }
        public int UnityDownloadCount
        {
            get => unityDownloadCount;
            set => this.RaiseAndSetIfChanged(ref unityDownloadCount, value);
        }
        public int SaturnDownloadCount
        {
            get => saturnDownloadCount;
            set => this.RaiseAndSetIfChanged(ref saturnDownloadCount, value);
        }
        public bool SaturnDownloading
        {
            get => saturnDownloading;
            set => this.RaiseAndSetIfChanged(ref saturnDownloading, value);
        }
        public bool UnityDownloading
        {
            get => unityDownloading;
            set => this.RaiseAndSetIfChanged(ref unityDownloading, value);
        }
        #endregion

        public DownloadManager()
        { }

        public void EnqueueAnime(EpisodeModel episode)
        {
            switch (episode.Host)
            {
                case Hosts.AnimeSaturn:
                    QueueAnimeSaturn.Add(new(episode));

                    log.Info($"[Saturn] Enqueued {episode.Title}");

                    if (!SaturnDownloading)
                    {
                        log.Debug($"[Saturn] Starting Downloading thread");
                        Task.Run(() => StartSaturnDownload());
                    }
                    break;

                case Hosts.AnimeUnity:
                    QueueAnimeUnity.Add(new(episode));

                    log.Info($"[Unity] Enqueued {episode.Title}");

                    if (!UnityDownloading)
                    {
                        log.Debug($"[Unity] Starting Downloading thread");
                        Task.Run(() => StartUnityDownload());
                    }
                    break;

                default:
                    // logging
                    break;
            }
        }

        private void StartSaturnDownload()
        {
            SaturnDownloading = true;

            while (QueueAnimeSaturn.Count != 0)
            {
                DownloadModel episode = QueueAnimeSaturn[0];

                log.Info($"[Saturn] Initializing Download: {episode.Episode.Title}");

                try
                {
                    episode.DownloadInfo = TenguApi.DownloadAsync(episode.Episode.DownloadUrl, episode.Episode.Host);

                    while (episode.DownloadInfo.Status == DownloadStatuses.Downloading)
                    {
                        if (episode.DownloadInfo.FileSize > episode.DownloadInfo.CurrentSize)
                            episode.DownloadPercentage = (int)(episode.DownloadInfo.CurrentSize * 100 / episode.DownloadInfo.FileSize);
                        
                        Task.Delay(50).Wait();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex, $"[Saturn] Download Exception: {episode.Episode.Title}");
                }
                finally
                {
                    episode.DownloadStatus = episode.DownloadInfo.Status;

                    log.Info($"[Saturn] Download Ended: {episode.Episode.Title} | STATUS: {episode.DownloadStatus}");
                    
                    QueueAnimeSaturn.Remove(episode);

                    log.Debug($"[Saturn] Episode Removed from Queue and Added to History: {episode.Episode.Title}");

                    // TODO
                    // History

                    episode = null;
                }
            }

            SaturnDownloading = false;

            log.Trace($"[Saturn] Queue Completed");
        }

        private void StartUnityDownload()
        {
            UnityDownloading = true;

            while (QueueAnimeUnity.Count != 0)
            {
                DownloadModel episode = QueueAnimeUnity[0];

                log.Info($"[Unity] Initializing Download: {episode.Episode.Title}");

                try
                {
                    episode.DownloadInfo = TenguApi.DownloadAsync(episode.Episode.DownloadUrl, episode.Episode.Host);

                    while (episode.DownloadInfo.Status == DownloadStatuses.Downloading)
                    {
                        if (episode.DownloadInfo.FileSize > episode.DownloadInfo.CurrentSize)
                            episode.DownloadPercentage = (int)(episode.DownloadInfo.CurrentSize * 100 / episode.DownloadInfo.FileSize);

                        Task.Delay(50).Wait();
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex, $"[Unity] Download Exception: {episode.Episode.Title}");
                }
                finally
                {
                    episode.DownloadStatus = episode.DownloadInfo.Status;

                    log.Info($"[Unity] Download Ended: {episode.Episode.Title} | STATUS: {episode.DownloadStatus}");

                    QueueAnimeUnity.Remove(episode);

                    log.Debug($"[Unity] Episode Removed from Queue and Added to History: {episode.Episode.Title}");

                    // TODO
                    // History
                    
                    episode = null;
                }
            }

            UnityDownloading = false;

            log.Trace($"[Unity] Queue Completed");
        }
    }
}
