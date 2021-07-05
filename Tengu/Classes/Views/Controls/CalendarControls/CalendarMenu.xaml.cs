using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Tengu.Classes.Views.Controls.CalendarControls
{
    /// <summary>
    /// Interaction logic for CalendarMenu.xaml
    /// </summary>
    public partial class CalendarMenu : UserControl
    {
        public CalendarMenu()
        {
            InitializeComponent();
        }

        #region Button Events
        private void btnMonday_Click(object sender, RoutedEventArgs e)
        {
            CalendarPage.main_calendar.NavigateToMondayControl();
        }

        private void btnTuesday_Click(object sender, RoutedEventArgs e)
        {
            CalendarPage.main_calendar.NavigateToTuesdayControl();
        }

        private void btnWednesday_Click(object sender, RoutedEventArgs e)
        {
            CalendarPage.main_calendar.NavigateToWednesdayControl();
        }

        private void btnSunday_Click(object sender, RoutedEventArgs e)
        {
            CalendarPage.main_calendar.NavigateToSundayControl();
        }

        private void btnThursday_Click(object sender, RoutedEventArgs e)
        {
            CalendarPage.main_calendar.NavigateToThursdayControl();
        }

        private void btnFriday_Click(object sender, RoutedEventArgs e)
        {
            CalendarPage.main_calendar.NavigateToFridayControl();
        }

        private void btnSaturday_Click(object sender, RoutedEventArgs e)
        {
            CalendarPage.main_calendar.NavigateToSaturdayControl();
        }
        #endregion
    }
}
