using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Interfaces;

namespace Tengu.ViewModels
{
    public class DownloadPageViewModel : ReactiveObject
    {
        public IDownloadManager downloadManager { get; private set; }

        public DownloadPageViewModel()
        {
            downloadManager = Locator.Current.GetService<IDownloadManager>();
        }
    }
}
