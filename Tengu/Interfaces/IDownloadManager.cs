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
        DownloadInfosModel DownloadInfoUnity { get; set; }
        DownloadInfosModel DownloadInfoSaturn { get; set; }

        void EnqueueAnime(EpisodeModel episode);
    }
}
