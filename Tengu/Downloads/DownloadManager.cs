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

namespace Tengu.Downloads
{
    public class DownloadManager : ReactiveObject, IDownloadManager
    {
        private static ITenguApi TenguApi => Locator.Current.GetService<ITenguApi>();

        private Queue<EpisodeModel> queueAnimeSaturn = new();
        private Queue<EpisodeModel> queueAnimeUnity = new();

        private int saturnDownloadPercentage = 0;

        private bool saturnDownloading = false;
        private bool unityDownloading = false;

        #region Properties
        public Queue<EpisodeModel> QueueAnimeSaturn
        {
            get => queueAnimeSaturn;
            set => this.RaiseAndSetIfChanged(ref queueAnimeSaturn, value);
        }
        public Queue<EpisodeModel> QueueAnimeUnity
        {
            get => queueAnimeUnity;
            set => this.RaiseAndSetIfChanged(ref queueAnimeUnity, value);
        }
        public int SaturnDownloadPercentage
        {
            get => saturnDownloadPercentage;
            set => this.RaiseAndSetIfChanged(ref saturnDownloadPercentage, value);
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
                    QueueAnimeSaturn.Enqueue(episode);

                    if(!SaturnDownloading)
                        Task.Run(() => StartSaturnDownload());
                    break;

                case Hosts.AnimeUnity:
                    QueueAnimeUnity.Enqueue(episode);

                    if(!UnityDownloading)
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

            try
            {
                while (QueueAnimeSaturn.Count != 0)
                {
                    SaturnDownloadPercentage = 0;

                    EpisodeModel episode = QueueAnimeSaturn.Dequeue();
                    
                    DownloadInfosModel infos = TenguApi.DownloadAsync(episode.DownloadUrl, episode.Host);

                    //await infos.EnsureDownloadCompletation();
                    while(infos.Status != DownloadStatuses.Completed)
                    {
                        if(infos.CurrentSize != 0 && infos.FileSize != 0)
                            SaturnDownloadPercentage = (int)(infos.CurrentSize * 100 / infos.FileSize);

                        Task.Delay(50).Wait();
                    }

                    // TODO => CLEAR
                    // History ??
                }
            }
            catch (Exception ex)
            {
                // LOG
            }
            finally
            {
                SaturnDownloading = false;
            }
        }

        private async Task StartUnityDownload()
        {
            UnityDownloading = true;

            while (QueueAnimeUnity.Count != 0)
            {
                EpisodeModel episode = QueueAnimeUnity.Dequeue();

                DownloadInfosModel infos = TenguApi.DownloadAsync(episode.DownloadUrl, episode.Host);

                // await infos.EnsureDownloadCompletation();

                // Clear
            }

            UnityDownloading = false;
        }
    }
}
