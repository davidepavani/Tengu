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

        private DownloadInfosModel downloadInfoSaturn;
        private DownloadInfosModel downloadInfoUnity;

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
        public DownloadInfosModel DownloadInfoUnity
        {
            get => downloadInfoUnity;
            set => this.RaiseAndSetIfChanged(ref downloadInfoUnity, value);
        }
        public DownloadInfosModel DownloadInfoSaturn
        {
            get => downloadInfoSaturn;
            set => this.RaiseAndSetIfChanged(ref downloadInfoSaturn, value);
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

                    if(!saturnDownloading)
                        Task.Run(() => StartSaturnDownload());
                    break;

                case Hosts.AnimeUnity:
                    QueueAnimeUnity.Enqueue(episode);

                    if(!unityDownloading)
                        Task.Run(() => StartUnityDownload());
                    break;

                default:
                    // logging
                    break;
            }
        }

        private async Task StartSaturnDownload()
        {
            saturnDownloading = true;

            while (QueueAnimeSaturn.Count != 0)
            {
                EpisodeModel episode = QueueAnimeSaturn.Dequeue();

                DownloadInfoSaturn = TenguApi.DownloadAsync(episode.Id, episode.Host);

                await DownloadInfoSaturn.EnsureDownloadCompletation();

                // TODO => CLEAR
                // History ??
                DownloadInfoSaturn = null;
            }

            saturnDownloading = false;
        }

        private async Task StartUnityDownload()
        {
            unityDownloading = true;

            while (QueueAnimeUnity.Count != 0)
            {
                EpisodeModel episode = QueueAnimeUnity.Dequeue();

                DownloadInfoUnity = TenguApi.DownloadAsync(episode.Id, episode.Host);

                await DownloadInfoUnity.EnsureDownloadCompletation();

                // Clear
                DownloadInfoUnity = null;
            }

            unityDownloading = false;
        }
    }
}
