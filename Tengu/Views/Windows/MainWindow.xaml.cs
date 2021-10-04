using HandyControl.Tools;
using NLog;
using System;
using System.Collections.Generic;
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
using Tengu.ViewModels;
using Tengu.WebScrapper;
using Logger = NLog.Logger;

namespace Tengu.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : HandyControl.Controls.Window
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public MainWindow()
        {
            SplashWindow.Instance.AddMessage("STARTING");

            string error_message = string.Empty;
            bool retry;

            do
            {
                retry = false;

                int attempt = 0;
                bool isConnected = false;

                do
                {
                    isConnected = ApplicationHelper.IsConnectedToInternet();
                    log.Info("Attempt: " + attempt + " - Is Connected to Internet: " + isConnected);

                    if (!isConnected)
                    {
                        for (int i = 3; i > 0; i--)
                        {
                            string mx = string.Format("CONNECTION FAILED!\nRETRY IN {0} SECONDS", i);
                            SplashWindow.Instance.AddMessage(mx);

                            Thread.Sleep(1000);
                        }

                        attempt++;
                    }

                } while (!isConnected && attempt < 1);

                if (isConnected)
                {
                    ScrapperService.Instance.InitializeDriver(out error_message);
                }

                if(!isConnected || !string.IsNullOrEmpty(error_message))
                {
                    SplashWindow.Instance.AddMessage("ERROR");

                    if (new InitErrorDialog(error_message).ShowDialog() == true)
                    {
                        retry = true;

                        SplashWindow.Instance.AddMessage("RETRYING");
                    }
                    else
                    {
                        log.Error("Closing Application..");

                        // Close Driver and Application
                        ScrapperService.Instance.CloseDriver();
                        Environment.Exit(-1);
                    }
                }

            } while (retry);

            SplashWindow.Instance.AddMessage("COMPLETED");

            InitializeComponent();

            SplashWindow.Instance.LoadComplete();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            (DataContext as MainWindowViewModel).Navigate("MenuUserControl");
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if ((DataContext as MainWindowViewModel).IsDownloading)
            {
                if (new CustomDialog(this, "There are downloads in progress!\nAre you sure you want to cancel them?").ShowDialog() == true)
                {
                    // Kill download Worker
                    (DataContext as MainWindowViewModel).AbortDownloads();

                    log.Info("Downloads Aborted!");

                    e.Cancel = false;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }

            // Closing Driver
            ScrapperService.Instance.CloseDriver();
            log.Info("Driver Closed .. ");

            // Save App Configuration
            ProgramInfos.Instance.SaveConfiguration(); 
        }
    }
}
