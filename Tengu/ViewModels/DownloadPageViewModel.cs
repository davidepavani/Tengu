using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Interfaces;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class DownloadPageViewModel : ReactiveObject
    {
        private readonly ITenguApi tenguApi;
        private readonly IProgramConfiguration configuration;

        public IDownloadManager downloadManager { get; set; }
        public ICommand CancelDownloadCommand { get; private set; }

        public DownloadPageViewModel()
        {
            downloadManager = Locator.Current.GetService<IDownloadManager>();
            tenguApi = Locator.Current.GetService<ITenguApi>();
            configuration = Locator.Current.GetService<IProgramConfiguration>();

            CancelDownloadCommand = ReactiveCommand.Create<DownloadModel>(CancelDownload);
            
            tenguApi.DownloadPath = string.IsNullOrEmpty(configuration.DownloadDirectory) ? 
                    Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) :
                        configuration.DownloadDirectory;
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
