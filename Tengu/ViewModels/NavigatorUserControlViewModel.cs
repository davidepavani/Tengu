using HandyControl.Controls;
using HandyControl.Tools;
using Notification.Wpf;
using Prism;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Tengu.Enums;
using Tengu.Views.Controls;
using Unity;
using static Tengu.Utilities.PrismEvents;

namespace Tengu.ViewModels
{
    public class NavigatorUserControlViewModel : BindablePropertyBase, INavigationAware
    {
        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private NavigateDestination selected_item;

        private bool is_downloading;

        #region Properties
        public ICommand CmdNavigate { get; private set; }
        public ICommand CmdNavigateHome { get; private set; }
        public NavigateDestination SelectedItem
        {
            get { return selected_item; }
            set
            {
                if (selected_item != value)
                {
                    selected_item = value;
                    RaisePropertyChanged();
                }
            }
        }
        public bool IsDownloading
        {
            get { return is_downloading; }
            set
            {
                if (is_downloading != value)
                {
                    is_downloading = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion

        #region Constructors
        public NavigatorUserControlViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base()
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            CmdNavigate = new DelegateCommand<string>(Navigate);
            CmdNavigateHome = new DelegateCommand<string>(NavigateHome);

            _eventAggregator.GetEvent<DownloadEvent>().Subscribe(Downloading);
        }
        public NavigatorUserControlViewModel() { }
        #endregion

        public void Downloading(bool bDownload)
        {
            IsDownloading = bDownload;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            SelectedItem = (NavigateDestination)navigationContext.Parameters["NavigateDestination"];

            Navigate(SelectedItem.GetControlNameFromEnum());
        }

        public void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate("ControlsRegion", navigatePath);
            }
        }

        public void NavigateHome(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate("MainRegion", navigatePath);
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true; // throw new NotImplementedException();
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // throw new NotImplementedException();
        }
    }
}
