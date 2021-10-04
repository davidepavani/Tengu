using HandyControl.Controls;
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
using Tengu.Models;
using static Tengu.Utilities.PrismEvents;

namespace Tengu.ViewModels.DownloadControlsViewModels
{
    public class HistoryUserControlViewModel : BindablePropertyBase
    {
        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;
        private OptimizedObservableCollection<HistoryData> download_history;

        #region Properties
        public ICommand CommandClearHistory { get; private set; }
        public ICommand CommandNavigateToQueue { get; private set; }

        public OptimizedObservableCollection<HistoryData> DownloadHistory
        {
            get { return download_history; }
            set
            {
                download_history = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public HistoryUserControlViewModel() { }
        public HistoryUserControlViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            CommandClearHistory = new SimpleRelayCommand(ClearHistory);
            CommandNavigateToQueue = new SimpleRelayCommand(Navigate);

            DownloadHistory = new OptimizedObservableCollection<HistoryData>();

            _eventAggregator.GetEvent<AddAnimeToDownloadHistoryEvent>().Subscribe(AddToHistory);
        }

        public void Navigate()
        {
            _regionManager.RequestNavigate("DwnldFrame", "QueueUserControl");
        }

        public void ClearHistory()
        {
            if (DownloadHistory != null)
            {
                if (DownloadHistory.Count > 0)
                {
                    DownloadHistory.Clear();
                }
            }
        }

        public void AddToHistory(HistoryData history)
        {
            DownloadHistory.Add(history);
        }
    }
}
