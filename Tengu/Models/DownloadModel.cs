using Downla;
using Downla.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Models;

namespace Tengu.Models
{
    public class DownloadModel : ReactiveObject
    {
        public DownloadMonitor DownloadInfo { get; set; }
        public EpisodeModel Episode { get; private set; }

        public CancellationTokenSource TokenSource { get; set; }

        public DownloadModel(EpisodeModel episode)
        {
            Episode = episode;
        }
    }
}
