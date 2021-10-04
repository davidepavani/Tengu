using HandyControl.Tools;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Tengu.Utilities.PrismEvents;

namespace Tengu.ViewModels
{
    public class MainWindowViewModel : BindablePropertyBase
    {
        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private bool is_donwloading;

        public bool IsDownloading
        {
            get { return is_donwloading; }
            set
            {
                is_donwloading = value;
                RaisePropertyChanged();
            }
        }

        #region Constructors
        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base()
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<DownloadEvent>().Subscribe(DownloadChanged);
        }
        public MainWindowViewModel() { }
        #endregion

        private void DownloadChanged(bool bDownload)
        {
            IsDownloading = bDownload;
        }

        public void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate("MainRegion", navigatePath);
            }
        }

        public void AbortDownloads()
        {
            _eventAggregator.GetEvent<AbortAllDownloadsEvent>().Publish();
        }
    }
}
