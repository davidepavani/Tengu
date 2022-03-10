using Downla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Interfaces
{
    public interface IDownloadManager
    {
        Queue<EpisodeModel> QueueAnimeSaturn { get; set; }
        Queue<EpisodeModel> QueueAnimeUnity { get; set; }
        int SaturnDownloadPercentage { get; set; }
        bool UnityDownloading { get; set; }
        bool SaturnDownloading { get; set; }

        void EnqueueAnime(EpisodeModel episode);
    }
}
