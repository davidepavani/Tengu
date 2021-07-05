using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tengu.Classes.ViewModels;
using Tengu.Classes.Views.Controls.CalendarControls;
using Tengu.Classes.Views.Windows;
using Tengu.Classes.Enums;

namespace Tengu.Classes.Views.Controls
{
    /// <summary>
    /// Interaction logic for CalendarPage.xaml
    /// </summary>
    public partial class CalendarPage : UserControl
    {
        private List<DayControl> week_list;

        internal static CalendarPage main_calendar;

        public CalendarPage()
        {
            InitializeComponent();
            main_calendar = this;

            Initialize();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            NavigateToCalendarMenu();
        }

        private void Initialize()
        {
            week_list = new List<DayControl>()
            {
                new DayControl(CalendarDay.Monday),
                new DayControl(CalendarDay.Tuesday),
                new DayControl(CalendarDay.Wednesday),
                new DayControl(CalendarDay.Thursday),
                new DayControl(CalendarDay.Friday),
                new DayControl(CalendarDay.Saturday),
                new DayControl(CalendarDay.Sunday)
            };
        }

        #region Navigation Methods
        public void NavigateToCalendarMenu()
        {
            CalendarFrame.Navigate(new CalendarMenu());
        }
        public void NavigateToMondayControl()
        {
            CalendarFrame.Navigate(week_list[0]);
        }
        public void NavigateToTuesdayControl()
        {
            CalendarFrame.Navigate(week_list[1]);
        }
        public void NavigateToWednesdayControl()
        {
            CalendarFrame.Navigate(week_list[2]);
        }
        public void NavigateToThursdayControl()
        {
            CalendarFrame.Navigate(week_list[3]);
        }
        public void NavigateToFridayControl()
        {
            CalendarFrame.Navigate(week_list[4]);
        }
        public void NavigateToSaturdayControl()
        {
            CalendarFrame.Navigate(week_list[5]);
        }
        public void NavigateToSundayControl()
        {
            CalendarFrame.Navigate(week_list[6]);
        }
        #endregion

        private void btnLatestEpisodes_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigatoToLatestEpisodesPage();
        }
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToHomePage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToSearchPage();
        }

        private void btnDownloads_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToDownloadPage();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToSettingsPage();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToAboutPage();
        }
    }
}