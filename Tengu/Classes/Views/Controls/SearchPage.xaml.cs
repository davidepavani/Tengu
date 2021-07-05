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
using Tengu.Classes.DataModels;
using Tengu.Classes.ViewModels;
using Tengu.Classes.Views.Controls.SearchControls;
using Tengu.Classes.Views.Windows;

namespace Tengu.Classes.Views.Controls
{
    /// <summary>
    /// Interaction logic for SearchPage.xaml
    /// </summary>
    public partial class SearchPage : UserControl
    {
        internal static SearchPage search_page;

        public SearchPage()
        {
            InitializeComponent();

            search_page = this;
            this.DataContext = new SearchViewModel();
        }
        public SearchPage(string search_pattern)
        {
            InitializeComponent();
            sbAnime.Text = search_pattern;

            search_page = this;
            this.DataContext = new SearchViewModel(search_pattern);
        }

        private void sbAnime_SearchStarted(object sender, HandyControl.Data.FunctionEventArgs<string> e)
        {
            search_pagination.PageIndex = 1;
        }

        private void Pagination_PageUpdated(object sender, HandyControl.Data.FunctionEventArgs<int> e)
        {
            (DataContext as SearchViewModel).UpdatePagination(e.Info);
        }

        #region Navigation Events
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

        private void btnSearchedAnime_Click(object sender, RoutedEventArgs e)
        {
            AnimeData data = (sender as Button).DataContext as AnimeData;
            Dialog.Show(new DialogSearchedAnime(data));
        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
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
        #endregion
    }
}
