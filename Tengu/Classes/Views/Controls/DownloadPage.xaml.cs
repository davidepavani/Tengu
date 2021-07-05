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
using Tengu.Classes.ViewModels;
using Tengu.Classes.Views.Controls.DownloadControls;
using Tengu.Classes.Views.Windows;

namespace Tengu.Classes.Views.Controls
{
    /// <summary>
    /// Interaction logic for DownloadPage.xaml
    /// </summary>
    public partial class DownloadPage : UserControl
    {
        private HistoryControl history_control;
        private QueueControl queue_control;
        
        internal static DownloadPage main_download;
        public DownloadPage()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            main_download = this;
            
            queue_control = new QueueControl();
            history_control = new HistoryControl();

            NavigatoToQueue();
        }

        #region Navigation Download Frame
        public void NavigatoToQueue()
        {
            DwnldFrame.Navigate(queue_control);
        }
        public void NavigateToHistory()
        {
            DwnldFrame.Navigate(history_control);
        }
        #endregion

        #region Navigation Main Frame
        private void btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToCalendarPage();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToHomePage();
        }

        private void btnLatestEpisodes_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigatoToLatestEpisodesPage();
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToSearchPage();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToSettingsPage();
        }

        private void btnAbout_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToAboutPage();
        }
        #endregion
    }
}
