using Avalonia.Collections;
using Downla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Models;
using Tengu.Utilities;

namespace Tengu.Interfaces
{
    public interface IDownloadManager
    {
        AvaloniaList<DownloadModel> QueueAnimeSaturn { get; set; }
        AvaloniaList<DownloadModel> QueueAnimeUnity { get; set; }
        int UnityDownloadCount { get; set; }
        int SaturnDownloadCount { get; set; }
        bool SaturnDownloading { get; set; }
        bool UnityDownloading { get; set; }
        bool IsDownloading { get; set; }

        void DequeueAnime(DownloadModel episode);
        void EnqueueAnime(EpisodeModel episode);
    }
}
