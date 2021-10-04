using HandyControl.Tools;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Enums;
using Tengu.Utilities;
using static Tengu.Utilities.PrismEvents;

namespace Tengu.ViewModels
{
    public class MenuUserControlViewModel : BindablePropertyBase
    {
        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        private bool is_downloading;

        #region Properties
        public string ApplicationName { get; private set; }
        public ICommand CmdNavigate { get; private set; }
        public bool IsDownloading
        {
            get { return is_downloading; }
            set
            {
                is_downloading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public MenuUserControlViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base()
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            
            ApplicationName = ProgramInfos.DISPLAY_NAME;
            IsDownloading = false;
            CmdNavigate = new DelegateCommand<string>(Navigate);

            _eventAggregator.GetEvent<DownloadEvent>().Subscribe(Downloading);
        }
        public MenuUserControlViewModel() { }
        #endregion

        public void Downloading(bool bDownload)
        {
            IsDownloading = bDownload;
        }

        public void SearchNavigate(string search_pattern)
        {
            // Define navigation parameters
            NavigationParameters navigationParams = new();
            navigationParams.Add("NavigateDestination", NavigateDestination.Search);

            if (!string.IsNullOrEmpty(search_pattern))
            {
                _eventAggregator.GetEvent<ExecuteSearchEvent>().Publish(search_pattern);
            }

            _regionManager.RequestNavigate("MainRegion", "NavigatorUserControl", navigationParams);
        }

        public void Navigate(string navigate_destination)
        {
            // Define navigation parameters
            NavigationParameters navigationParams = new();
            navigationParams.Add("NavigateDestination", navigate_destination.ParseEnum<NavigateDestination>());

            if (navigate_destination != null)
            {
                _regionManager.RequestNavigate("MainRegion", "NavigatorUserControl", navigationParams);
            }
        }
    }
}
