using Avalonia.Collections;
using Avalonia.Threading;
using Downla;
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
using Tengu.Models;
using Tengu.Utilities;

namespace Tengu.Downloads
{
    public class DownloadManager : ReactiveObject, IDownloadManager
    {
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

                    if (!SaturnDownloading)
                        Task.Run(() => StartSaturnDownload());
                    break;

                case Hosts.AnimeUnity:
                    QueueAnimeUnity.Add(new(episode));

                    if (!UnityDownloading)
                        Task.Run(() => StartUnityDownload());
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

                try
                {
                    episode.DownloadInfo = TenguApi.DownloadAsync(episode.Episode.DownloadUrl, episode.Episode.Host);

                    while (episode.DownloadInfo.Status == DownloadStatuses.Downloading )
                    {
                        if (episode.DownloadInfo.FileSize > episode.DownloadInfo.CurrentSize)
                            episode.DownloadPercentage = (int)(episode.DownloadInfo.CurrentSize * 100 / episode.DownloadInfo.FileSize);
                        
                        Task.Delay(50).Wait();
                    }
                }
                catch (Exception ex)
                {
                    // LOG
                }
                finally
                {
                    episode.DownloadStatus = episode.DownloadInfo.Status;

                    QueueAnimeSaturn.Remove(episode);
                    episode = null;
                }
            }

            SaturnDownloading = false;
        }

        private void StartUnityDownload()
        {
            UnityDownloading = true;

            while (QueueAnimeUnity.Count != 0)
            {
                DownloadModel episode = QueueAnimeUnity[0];

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
                    // LOG
                }
                finally
                {
                    episode.DownloadStatus = episode.DownloadInfo.Status;

                    QueueAnimeUnity.Remove(episode);
                    episode = null;
                }
            }

            UnityDownloading = false;
        }
    }
}
