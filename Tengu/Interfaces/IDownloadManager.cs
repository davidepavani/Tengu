using Downla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Utilities;

namespace Tengu.Interfaces
{
    public interface IDownloadManager
    {
        CustomObservableCollection<EpisodeModel> QueueAnimeSaturn { get; set; }
        CustomObservableCollection<EpisodeModel> QueueAnimeUnity { get; set; }
        EpisodeModel UnityDownloadingEpisode { get; set; }
        EpisodeModel SaturnDownloadingEpisode { get; set; }
        int SaturnDownloadPercentage { get; set; }
        int UnityDownloadPercentage { get; set; }
        bool UnityDownloading { get; set; }
        bool SaturnDownloading { get; set; }

        void EnqueueAnime(EpisodeModel episode);
    }
}
