using Downla;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Models;

namespace Tengu.Models
{
    public class DownloadModel : ReactiveObject
    {
        private int downloadPercentage = 0;
        private DownloadStatuses downloadStatus = DownloadStatuses.Downloading;

        public DownloadInfosModel DownloadInfo { get; set; }
        public EpisodeModel Episode { get; private set; }
        public int DownloadPercentage
        {
            get => downloadPercentage;
            set => this.RaiseAndSetIfChanged(ref downloadPercentage, value);
        }
        public DownloadStatuses DownloadStatus
        {
            get => downloadStatus;
            set => this.RaiseAndSetIfChanged(ref downloadStatus, value);
        }

        public DownloadModel(EpisodeModel episode)
        {
            Episode = episode;
        }
    }
}
