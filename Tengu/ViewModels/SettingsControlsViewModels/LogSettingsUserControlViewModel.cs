using HandyControl.Tools;
using HandyControl.Tools.Command;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using static Tengu.Utilities.PrismEvents;

namespace Tengu.ViewModels.SettingsControlsVireModels
{
    class LogSettingsUserControlViewModel : BindablePropertyBase
    {
        private IEventAggregator _eventAggregator;
        private readonly IRegionManager _regionManager;

        private bool is_downloading;

        public ICommand CommandPrevSett { get; private set; }
        public ICommand CommandNextSett { get; private set; }
        public bool IsDownloading
        {
            get { return is_downloading; }
            set
            {
                is_downloading = value;
                RaisePropertyChanged();
            }
        }

        public LogSettingsUserControlViewModel(IEventAggregator eventAggregator, IRegionManager regionManager) : base()
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            IsDownloading = false;

            _eventAggregator.GetEvent<DownloadEvent>().Subscribe(Downloading);

            CommandPrevSett = new SimpleRelayCommand(PreviousSetting);
            CommandNextSett = new SimpleRelayCommand(NextSetting);
        }
        public LogSettingsUserControlViewModel() { }

        public void Downloading(bool bDownload) => IsDownloading = bDownload;

        private void NextSetting()
        {
            // Do Nothing .. 
        }
        private void PreviousSetting()
        {
            Navigate("DownloadSettingsUserControl");
        }

        public void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate("SettingsFrame", navigatePath);
            }
        }
    }
}
