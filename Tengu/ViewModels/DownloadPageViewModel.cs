using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Business.Commons;
using Tengu.Interfaces;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class DownloadPageViewModel : ReactiveObject
    {
        public IDownloadManager downloadManager { get; private set; }
        public ICommand CancelDownloadCommand { get; private set; }

        public DownloadPageViewModel()
        {
            downloadManager = Locator.Current.GetService<IDownloadManager>();

            CancelDownloadCommand = ReactiveCommand.Create<DownloadModel>(CancelDownload);
        }

        private void CancelDownload(DownloadModel episode)
        {
            if(episode != null)
            {
                downloadManager.DequeueAnime(episode);
            }
        }
    }
}
