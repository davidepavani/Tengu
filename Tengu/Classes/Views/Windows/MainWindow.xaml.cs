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
using System.ComponentModel;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tengu.Classes.Views.Controls;
using Tengu.Classes.WebScrapping;
using Tengu.Classes.ViewModels;
using HandyControl.Controls;
using System.Threading;
using Tengu.Classes.Logger;
using Notification.Wpf;

namespace Tengu.Classes.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        private HomePage home_page;
        private LatestEpisodesPage latest_episodes_page;
        private CalendarPage calendar_page;
        private DownloadPage download_page;
        private SearchPage search_page;
        private AnimeCardPage anime_card_page;
        private SettingsPage settings_page;
        private AboutPage about_page;

        private NotificationManager notificationManager;

        internal static MainWindow main_window;

        public MainWindow()
        {
            SplashWindow.Instance.AddMessage("STARTING");

            ProgramInfo.Instance.LoadConfiguration();

            // Declarations
            string error_message;
            bool retry;

            do
            {
                retry = false;
                
                if (!Scrapper.Instance.InitializeDriver(out error_message))
                {
                    SplashWindow.Instance.AddMessage("ERROR");

                    if (new InitErrorDialog(error_message).ShowDialog() == true)
                    {
                        retry = true;

                        SplashWindow.Instance.AddMessage("RETRYING");
                    }
                    else
                    {
                        WriteError("Closing Application..");

                        // Close Application
                        Scrapper.Instance.CloseDriver();
                        this.Close();
                    }
                }

            } while (retry);

            SplashWindow.Instance.AddMessage("COMPLETED");

            InitializeComponent();
            main_window = this;

            SplashWindow.Instance.LoadComplete();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Initialize();
            NavigateToHomePage();

            // Bring to front
            this.Activate();
            //this.Topmost = true;
        }

        private void Initialize()
        {
            notificationManager = new NotificationManager();
            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            Scrapper.Instance.RefreshCalendar();

            home_page = new HomePage();
            latest_episodes_page = new LatestEpisodesPage();
            calendar_page = new CalendarPage();
            download_page = new DownloadPage();
            search_page = new SearchPage();
            settings_page = new SettingsPage();
            about_page = new AboutPage();
        }

        public void NavigateToHomePage()
        {
            MainFrame.Navigate(home_page);
        }
        public void NavigatoToLatestEpisodesPage()
        {
            MainFrame.Navigate(latest_episodes_page);
        }
        public void NavigateToCalendarPage()
        {
            MainFrame.Navigate(calendar_page);
        }
        public void NavigateToDownloadPage()
        {
            MainFrame.Navigate(download_page);
        }
        public void NavigateToSearchPage()
        {
            MainFrame.Navigate(search_page);
        }
        public void NavigateToSearchPage(string search_pattern)
        {
            // Reinitialize the Control
            search_page = new SearchPage(search_pattern);

            MainFrame.Navigate(search_page);
        }
        public void NavigateToAnimeCardPage(string url)
        {
            // Reinitialize the Control
            anime_card_page = new AnimeCardPage(url);

            MainFrame.Navigate(anime_card_page);
        }
        public void NavigateToSettingsPage()
        {
            MainFrame.Navigate(settings_page);
        }
        public void NavigateToAboutPage()
        {
            MainFrame.Navigate(about_page);
        }
        public void NavigateBack()
        {
            MainFrame.GoBack();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
                // Security..
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (DownloadViewModel.Instance.IsDownloading)
            {
                if (new CustomDialog(this, "There are downloads in progress!\nAre you sure you want to cancel them?").ShowDialog() == true)
                {
                    // Kill download Worker
                    DownloadViewModel.Instance.AbortDownloadWorker();

                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

            Scrapper.Instance.CloseDriver(); // Closing Driver
            Log.Instance.CloseLog(); // Dispose Log
            ProgramInfo.Instance.SaveConfiguration(); // Save App Configuration
        }

        #region Global Notification
        public void ShowErrorNotification(string title, string message)
        {
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Error
            });
        }
        public void ShowSuccessNotification(string title, string message)
        {
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Success
            });
        }
        #endregion

        #region Debug & Logs
        private void WriteLog(string message)
        {
            Log.Instance.WriteLog("MainW", message);
        }
        private void WriteError(string message)
        {
            Log.Instance.WriteError("MainW", message);
        }
        #endregion
    }
}
