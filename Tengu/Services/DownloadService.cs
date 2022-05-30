using Avalonia.Collections;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API;
using Tengu.Models;

namespace Tengu.Services
{
    public class DownloadService : ReactiveObject
    {
        private static ITenguApi TenguApi => Locator.Current.GetService<ITenguApi>();

        private AvaloniaList<DownloadModel> animeSaturnQueue = new();
        private AvaloniaList<DownloadModel> animeUnityQueue = new();

        private DownloadModel currentSaturnDownload  = null;
        private DownloadModel currentUnityDownload = null;

        #region Properties
        public AvaloniaList<DownloadModel> AnimeSaturnQueue
        {
            get => animeSaturnQueue;
            set => this.RaiseAndSetIfChanged(ref animeSaturnQueue, value);
        }
        public AvaloniaList<DownloadModel> AnimeUnityQueue
        {
            get => animeUnityQueue;
            set => this.RaiseAndSetIfChanged(ref animeUnityQueue, value);
        }
        public DownloadModel CurrentUnityDownload 
        { 
            get => currentUnityDownload; 
            set => this.RaiseAndSetIfChanged(ref currentUnityDownload, value);
        }
        public DownloadModel CurrentSaturnDownload 
        { 
            get => currentSaturnDownload; 
            set => this.RaiseAndSetIfChanged(ref currentSaturnDownload, value);
        }
        #endregion
    }
}
