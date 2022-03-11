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
using Tengu.Utilities;

namespace Tengu.Downloads
{
    public class DownloadManager : ReactiveObject, IDownloadManager
    {
        private static ITenguApi TenguApi => Locator.Current.GetService<ITenguApi>();

        private CustomObservableCollection<EpisodeModel> queueAnimeSaturn = new();
        private CustomObservableCollection<EpisodeModel> queueAnimeUnity = new();

        private EpisodeModel saturnDownloadingEpisode;
        private EpisodeModel unityDownloadingEpisode;
        private int saturnDownloadPercentage = 0;
        private int unityDownloadPercentage = 0;

        private int saturnDownloadCount = 0;
        private int unityDownloadCount = 0;

        private bool saturnDownloading = false;
        private bool unityDownloading = false;

        #region Properties
        public EpisodeModel SaturnDownloadingEpisode
        {
            get => saturnDownloadingEpisode;
            set => this.RaiseAndSetIfChanged(ref saturnDownloadingEpisode, value);
        }
        public EpisodeModel UnityDownloadingEpisode
        {
            get => unityDownloadingEpisode;
            set => this.RaiseAndSetIfChanged(ref unityDownloadingEpisode, value);
        }
        public CustomObservableCollection<EpisodeModel> QueueAnimeSaturn
        {
            get => queueAnimeSaturn;
            set => this.RaiseAndSetIfChanged(ref queueAnimeSaturn, value);
        }
        public CustomObservableCollection<EpisodeModel> QueueAnimeUnity
        {
            get => queueAnimeUnity;
            set => this.RaiseAndSetIfChanged(ref queueAnimeUnity, value);
        }
        public int SaturnDownloadPercentage
        {
            get => saturnDownloadPercentage;
            set => this.RaiseAndSetIfChanged(ref saturnDownloadPercentage, value);
        }
        public int SaturnDownloadCount
        {
            get => saturnDownloadCount;
            set => this.RaiseAndSetIfChanged(ref saturnDownloadCount, value);
        }
        public int UnityDownloadCount
        {
            get => unityDownloadCount;
            set => this.RaiseAndSetIfChanged(ref unityDownloadCount, value);
        }
        public int UnityDownloadPercentage
        {
            get => unityDownloadPercentage;
            set => this.RaiseAndSetIfChanged(ref unityDownloadPercentage, value);
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
                    QueueAnimeSaturn.Add(episode);
                    SaturnDownloadCount = QueueAnimeSaturn.Count + (SaturnDownloadingEpisode != null ? 1 : 0);

                    if (!SaturnDownloading)
                        Task.Run(() => StartSaturnDownload());
                    break;

                case Hosts.AnimeUnity:
                    QueueAnimeUnity.Add(episode);
                    UnityDownloadCount = QueueAnimeUnity.Count + (UnityDownloadingEpisode != null ? 1 : 0);

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
                try
                {
                    SaturnDownloadPercentage = 0;

                    SaturnDownloadingEpisode = QueueAnimeSaturn[0];
                    QueueAnimeSaturn.RemoveAt(0);

                    DownloadInfosModel infos = TenguApi.DownloadAsync(SaturnDownloadingEpisode.DownloadUrl, SaturnDownloadingEpisode.Host);

                    while (infos.Status != DownloadStatuses.Completed)
                    {
                        if (infos.CurrentSize != 0 && infos.FileSize != 0)
                            SaturnDownloadPercentage = (int)(infos.CurrentSize * 100 / infos.FileSize);

                        Task.Delay(50).Wait();
                    }

                    // TODO => CLEAR
                    // History ??
                    infos = null;
                }
                catch (Exception ex)
                {
                    // LOG
                }
                finally
                {
                    SaturnDownloadingEpisode = null;
                }

                SaturnDownloadCount = QueueAnimeSaturn.Count + (SaturnDownloadingEpisode != null ? 1 : 0);
            }

            SaturnDownloading = false;
        }

        private void StartUnityDownload()
        {
            UnityDownloading = true;

            while (QueueAnimeUnity.Count != 0)
            {
                try
                {
                    UnityDownloadPercentage = 0;

                    UnityDownloadingEpisode = QueueAnimeUnity[0];
                    QueueAnimeUnity.RemoveAt(0);

                    DownloadInfosModel infos = TenguApi.DownloadAsync(UnityDownloadingEpisode.DownloadUrl, UnityDownloadingEpisode.Host);

                    while (infos.Status != DownloadStatuses.Completed)
                    {
                        if (infos.CurrentSize != 0 && infos.FileSize != 0)
                            UnityDownloadPercentage = (int)(infos.CurrentSize * 100 / infos.FileSize);

                        Task.Delay(50).Wait();
                    }

                    // TODO => CLEAR
                    // History ??
                    infos = null;
                }
                catch (Exception ex)
                {
                    // LOG
                }
                finally
                {
                    UnityDownloadingEpisode = null;
                }

                UnityDownloadCount = QueueAnimeUnity.Count + (UnityDownloadingEpisode != null ? 1 : 0);
            }

            UnityDownloading = false;
        }
    }
}
