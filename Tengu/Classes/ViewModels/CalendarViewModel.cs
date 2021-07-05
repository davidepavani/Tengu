using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Classes.Enums;
using Tengu.Classes.Logger;
using Tengu.Classes.Views.Controls;
using Tengu.Classes.Views.Windows;
using Tengu.Classes.WebScrapping;

namespace Tengu.Classes.ViewModels
{
    public class CalendarViewModel : BindablePropertyBase
    {
        #region Declarations
        private OptimizedObservableCollection<string> source_list;
        private readonly CalendarDay day;

        private bool is_loading;
        private bool refreshing;

        private BackgroundWorker refresh_worker;

        private ICommand command_next_day;
        private ICommand command_prev_day;
        private ICommand command_menu;
        private ICommand command_refresh;
        #endregion

        #region Properties
        public ICommand CommandRefresh
        {
            get { return command_refresh; }
            set { command_refresh = value; }
        }
        public ICommand CommandMenu
        {
            get { return command_menu; }
            set { command_menu = value; }
        }
        public ICommand CommandNextDay
        {
            get { return command_next_day; }
            set { command_next_day = value; }
        }
        public ICommand CommandPrevDay
        {
            get { return command_prev_day; }
            set { command_prev_day = value; }
        }
        public string Day
        {
            get { return day.ToString(); }
        }
        public OptimizedObservableCollection<string> SourceList
        {
            get { return source_list; }
            set
            {
                source_list = value;
                RaisePropertyChanged();
            }
        }
        public bool IsLoading
        {
            get { return is_loading; }
            set
            {
                is_loading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public CalendarViewModel(CalendarDay week_day)
        {
            SourceList = new OptimizedObservableCollection<string>();
            day = week_day;

            CommandMenu = new SimpleRelayCommand(new Action(ExecCommandMenu));
            CommandRefresh = new SimpleRelayCommand(new Action(ExecCommandRefresh));
            CommandNextDay = new SimpleRelayCommand(new Action(ExecCommandNextDay));
            CommandPrevDay = new SimpleRelayCommand(new Action(ExecCommandPrevDay));

            ExecCommandRefresh();
        }
        #endregion

        #region Public Methods
        public void RefreshList()
        {
            IsLoading = true;

            // Clear List
            SourceList.Clear();

            List<string> animes = Scrapper.Instance.GetDayAnimes(day);

            WriteLog("Calendar List Refreshed: " + day.ToString());

            if(animes != null)
            {
                foreach (string anime in animes)
                {
                    SourceList.Add(anime);
                }
            }

            IsLoading = false;
        }
        #endregion

        #region Private Methods
        private void ExecCommandMenu()
        {
            CalendarPage.main_calendar.NavigateToCalendarMenu();
        }
        private void ExecCommandRefresh()
        {
            if(!refreshing)
            {
                if(refresh_worker == null)
                {
                    refresh_worker = new BackgroundWorker();
                }

                refresh_worker.DoWork += Refresh_worker_DoWork;
                refresh_worker.RunWorkerCompleted += Refresh_worker_RunWorkerCompleted;

                refreshing = true;
                refresh_worker.RunWorkerAsync();
            }
        }

        private void ExecCommandNextDay()
        {
            switch (day)
            {
                case CalendarDay.Monday:
                    CalendarPage.main_calendar.NavigateToTuesdayControl();
                    break;
                case CalendarDay.Tuesday:
                    CalendarPage.main_calendar.NavigateToWednesdayControl();
                    break;
                case CalendarDay.Wednesday:
                    CalendarPage.main_calendar.NavigateToThursdayControl();
                    break;
                case CalendarDay.Thursday:
                    CalendarPage.main_calendar.NavigateToFridayControl();
                    break;
                case CalendarDay.Friday:
                    CalendarPage.main_calendar.NavigateToSaturdayControl();
                    break;
                case CalendarDay.Saturday:
                    CalendarPage.main_calendar.NavigateToSundayControl();
                    break;
                case CalendarDay.Sunday:
                    CalendarPage.main_calendar.NavigateToMondayControl();
                    break;
            }
        }
        private void ExecCommandPrevDay()
        {
            switch (day)
            {
                case CalendarDay.Monday:
                    CalendarPage.main_calendar.NavigateToSundayControl();
                    break;
                case CalendarDay.Tuesday:
                    CalendarPage.main_calendar.NavigateToMondayControl();
                    break;
                case CalendarDay.Wednesday:
                    CalendarPage.main_calendar.NavigateToTuesdayControl();
                    break;
                case CalendarDay.Thursday:
                    CalendarPage.main_calendar.NavigateToWednesdayControl();
                    break;
                case CalendarDay.Friday:
                    CalendarPage.main_calendar.NavigateToThursdayControl();
                    break;
                case CalendarDay.Saturday:
                    CalendarPage.main_calendar.NavigateToFridayControl();
                    break;
                case CalendarDay.Sunday:
                    CalendarPage.main_calendar.NavigateToSaturdayControl();
                    break;
            }
        }
        #endregion

        #region Refresh Worker
        private void Refresh_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                RefreshList();
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);

                ShowErrorMessage("Calendar Refresh Error!", ex.Message);
            }
            finally
            {
                refreshing = false;
                IsLoading = false;

                refresh_worker.DoWork -= Refresh_worker_DoWork; 
                refresh_worker.RunWorkerCompleted -= Refresh_worker_RunWorkerCompleted;
                refresh_worker.Dispose();
            }
        }

        private void Refresh_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IsLoading = true;

                // Scrapper: Refresh Calendar
                Scrapper.Instance.RefreshCalendar();
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);

                ShowErrorMessage("Calendar Refresh Error!", ex.Message);
            }
        }
        #endregion

        private void ShowErrorMessage(string title, string message)
        {
            MainWindow.main_window.ShowErrorNotification(title, message);
        }

        #region Debug & Logs
        private void WriteLog(string message)
        {
            Log.Instance.WriteLog("Calendar", message);
        }
        private void WriteError(string message)
        {
            Log.Instance.WriteError("Calendar", message);
        }
        #endregion
    }
}
