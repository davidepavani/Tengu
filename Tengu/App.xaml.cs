using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Unity;
using Tengu.Module;
using Tengu.ViewModels;
using Tengu.ViewModels.CalendarControlsViewModels;
using Tengu.ViewModels.DownloadControlsViewModels;
using Tengu.ViewModels.SettingsControlsVireModels;
using Tengu.Views.Controls;
using Tengu.Views.Controls.CalendarControls;
using Tengu.Views.Controls.DownloadControls;
using Tengu.Views.Controls.SettingsControls;
using Tengu.Views.Windows;

namespace Tengu
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        private const string MUTEX_NAME = "TenguMutex";
        private Mutex tengu_mutex;

        protected override void OnStartup(StartupEventArgs e)
        {
            tengu_mutex = new Mutex(true, MUTEX_NAME, out bool is_new_instance);

            if (!is_new_instance)
            {

                HandyControl.Controls.MessageBox.Show(
                    string.Format("A {0} instance is already running!", ProgramInfos.APPLICATION_NAME),
                    "Warning!",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                    );

                tengu_mutex.Dispose();
                Current.Shutdown();
            }
            else
            {
                HandyControl.Controls.SplashWindow.Init(() => {
                    SplashWindow splash = new SplashWindow();
                    return splash;
                });
                base.OnStartup(e);
            }
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<ModuleA>();
        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();

            // Windows
            ViewModelLocationProvider.Register<MainWindow, MainWindowViewModel>();

            // Controls
            ViewModelLocationProvider.Register<MenuUserControl, MenuUserControlViewModel>();
            ViewModelLocationProvider.Register<NavigatorUserControl, NavigatorUserControlViewModel>();
            ViewModelLocationProvider.Register<AboutUserControl, AboutUserControlViewModel>();
            ViewModelLocationProvider.Register<LatestEpisodesUserControl, LatestEpisodesUserControlViewModel>();
            ViewModelLocationProvider.Register<DownloadUserControl, DownloadUserControlViewModel>();
            ViewModelLocationProvider.Register<SearchUserControl, SearchUserControlViewModel>();
            ViewModelLocationProvider.Register<SettingsUserControl, SettingsUserControlViewModel>();
            ViewModelLocationProvider.Register<CalendarUserControl, CalendarUserControlViewModel>();
            ViewModelLocationProvider.Register<UpcomingAnimesUserControl, UpcomingAnimesUserControlViewModel>();
            ViewModelLocationProvider.Register<KitsuAnimeCardUserControl, KitsuAnimeCardUserControlViewModel>();
            ViewModelLocationProvider.Register<AnimeCardUserControl, AnimeCardUserControlViewModel>();

            // ------------------------- Sub Controls -------------------------

            // Settings Controls
            ViewModelLocationProvider.Register<DownloadSettingsUserControl, DownloadSettingsUserControlViewModel>();
            ViewModelLocationProvider.Register<LogSettingsUserControl, LogSettingsUserControlViewModel>();

            // Donwload Controls
            ViewModelLocationProvider.Register<QueueUserControl, QueueUserControlViewModel>();
            ViewModelLocationProvider.Register<HistoryUserControl, HistoryUserControlViewModel>();

            // CalendarControls
            ViewModelLocationProvider.Register<CalendarMenuUserControl, CalendarMenuUserControlViewModel>();
            ViewModelLocationProvider.Register<DayUserControl, DayUserControlViewModel>();
        }
    }
}
