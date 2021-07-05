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
using Tengu.Classes.Views.Windows;

namespace Tengu.Classes.Views.Controls
{
    /// <summary>
    /// Interaction logic for LatestEpisodesPage.xaml
    /// </summary>
    public partial class LatestEpisodesPage : UserControl
    {
        public LatestEpisodesPage()
        {
            InitializeComponent();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToHomePage();
        }

        private void btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToCalendarPage();
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToSearchPage();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToDownloadPage();
        }

        private void btnOptions_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToSettingsPage();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToAboutPage();
        }
    }
}
