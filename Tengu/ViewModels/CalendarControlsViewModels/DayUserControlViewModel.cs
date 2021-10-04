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
using Tengu.Shared.Enums;
using Tengu.WebScrapper;
using static Tengu.Utilities.PrismEvents;

namespace Tengu.ViewModels.CalendarControlsViewModels
{
    public class DayUserControlViewModel : BindablePropertyBase, INavigationAware
    {
        private OptimizedObservableCollection<string> source_list;

        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        private CalendarDay current_day;
        private bool is_refreshing;

        public ICommand NextDayCommand { get; private set; }
        public ICommand PrevDayCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ICommand GoBackToMenuCommand { get; private set; }

        #region Properties
        public OptimizedObservableCollection<string> SourceList
        {
            get { return source_list; }
            set
            {
                source_list = value;
                RaisePropertyChanged();
            }
        }
        public bool IsRefreshing
        {
            get { return is_refreshing; }
            set
            {
                is_refreshing = value;
                RaisePropertyChanged();
            }
        }
        public CalendarDay CurrentDay
        {
            get { return current_day; }
            set
            {
                current_day = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public DayUserControlViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) 
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;

            SourceList = new OptimizedObservableCollection<string>();

            IsRefreshing = false;

            NextDayCommand = new SimpleRelayCommand(NextDay);
            PrevDayCommand = new SimpleRelayCommand(PrevDay);
            RefreshCommand = new SimpleRelayCommand(Refresh);
            GoBackToMenuCommand = new SimpleRelayCommand(Navigate);

            _eventAggregator.GetEvent<RefreshingCalendarEvent>().Subscribe(CalendarRefreshing);
        }
        public DayUserControlViewModel() { }

        public void CalendarRefreshing(bool bRefreshing) => IsRefreshing = bRefreshing;

        public void Refresh()
        {
            if (!IsRefreshing)
            {
                IsRefreshing = true;
                _eventAggregator.GetEvent<RefreshCalendarEvent>().Publish();
            }
        }
        public void PrevDay()
        {
            if(CurrentDay == CalendarDay.Monday)
            {
                // Get Last
                CurrentDay = Enum.GetValues(typeof(CalendarDay)).Cast<CalendarDay>().Last();
            }
            else
            {
                // Get Prev
                int temp_index = (int)CurrentDay - 1;
                CurrentDay = (CalendarDay)Enum.ToObject(typeof(CalendarDay), temp_index);
            }

            RefreshList();
        }
        public void NextDay()
        {
            if (CurrentDay == CalendarDay.Sunday)
            {
                // Get First
                CurrentDay = Enum.GetValues(typeof(CalendarDay)).Cast<CalendarDay>().First();
            }
            else
            {
                // Get Prev
                int temp_index = (int)CurrentDay + 1;
                CurrentDay = (CalendarDay)Enum.ToObject(typeof(CalendarDay), temp_index);
            }

            RefreshList();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            CurrentDay = (CalendarDay)navigationContext.Parameters["Day"];

            RefreshList();
        }

        private void RefreshList()
        {
            // Refresh List
            SourceList.Clear();

            foreach(string ani in ScrapperService.Instance.GetDayAnimes(CurrentDay))
            {
                SourceList.Add(ani);
            }

            IsRefreshing = false;
        }

        public void Navigate()
        {
            _regionManager.RequestNavigate("CalendarFrame", "CalendarMenuUserControl");
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Do nothing ..
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
    }
}
