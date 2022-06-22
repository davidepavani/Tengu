using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Models;
using Tengu.Models;

namespace Tengu.Interfaces
{
    public interface IDownloadService
    {
        DownloadModel CurrentUnityDownload { get; set; }
        DownloadModel CurrentSaturnDownload { get; set; }
        int DownloadCount { get; set; }
        void EnqueueAnime(EpisodeModel episode);
    }
}
