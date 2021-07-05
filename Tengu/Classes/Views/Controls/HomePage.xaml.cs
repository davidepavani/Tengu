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
using HandyControl.Controls;
using HandyControl.Data;
using Tengu.Classes.DataModels;
using Tengu.Classes.ViewModels;
using Tengu.Classes.Views.Windows;

namespace Tengu.Classes.Views.Controls
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : UserControl
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void btnLatestEpisodes_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigatoToLatestEpisodesPage();
        }

        private void btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToCalendarPage();
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.main_window.NavigateToDownloadPage();
        }

        private void sbAnime_SearchStarted(object sender, FunctionEventArgs<string> e)
        {
            MainWindow.main_window.NavigateToSearchPage(e.Info);
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
