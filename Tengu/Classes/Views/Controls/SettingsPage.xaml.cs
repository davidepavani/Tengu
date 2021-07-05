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
using Tengu.Classes.Views.Controls.SettingsControls;
using Tengu.Classes.Views.Windows;

namespace Tengu.Classes.Views.Controls
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        private DownloadSettingsControl download_settings_page;
        private LogSettingsControl log_settings_page;

        internal static SettingsPage settings_page;
        public SettingsPage()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            settings_page = this;

            download_settings_page = new DownloadSettingsControl();
            log_settings_page = new LogSettingsControl();

            NavigateToDownloadSettingsPage();
        }

        public void NavigateToDownloadSettingsPage()
        {
            SettingsFrame.Navigate(download_settings_page);
        }
        public void NavigateToLogSettingsPage()
        {
            SettingsFrame.Navigate(log_settings_page);
        }

        #region Main Navigations
        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToHomePage();
        }

        private void btnLatestEpisodes_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigatoToLatestEpisodesPage();
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

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToAboutPage();
        }
        #endregion
    }
}