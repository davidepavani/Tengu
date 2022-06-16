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

namespace Tengu.Services
{
    public class DownloadService : ReactiveObject, IDownloadService
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.MainLogger);

        private static ITenguApi TenguApi => Locator.Current.GetService<ITenguApi>();

        private AvaloniaList<DownloadModel> AnimeQueue = new();
        private int downloadCount = 0;

        private DownloadModel currentSaturnDownload  = null;
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
        public int DownloadCount
        {
            get => downloadCount;
            set => this.RaiseAndSetIfChanged(ref downloadCount, value);
        }
        #endregion

        public void EnqueueAnime(EpisodeModel episode)
        {
            log.Info($"[Saturn] Enqueued {episode.Title}");

            AnimeQueue.Add(new(episode));
            DownloadCount = AnimeQueue.Count;
        }

        public void SaturnDownload()
        {
            DownloadModel anime = GetNextSaturnEpisode();

            while(anime != null)
            {

            }

            DownloadCount = AnimeQueue.Count;
        }

        private DownloadModel GetNextSaturnEpisode()
            => AnimeQueue.FirstOrDefault(x => x.Episode.Host.Equals(Hosts.AnimeSaturn), null);
    }
}
