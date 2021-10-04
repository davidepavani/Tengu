using Tengu.Views.Windows;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Views.Controls;
using Tengu.Views.Controls.SettingsControls;
using Tengu.Views.Controls.DownloadControls;
using Tengu.Views.Controls.CalendarControls;
using Prism.Regions;

namespace Tengu.Module
{
    public class ModuleA : IModule
    {
        private readonly IRegionManager _regionManager;

        public ModuleA(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Here the regions MUST be registered in the region manager for the navigation
            // Otherwise all controls will be placed in the main region

            // Main Region
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(MenuUserControl));
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(NavigatorUserControl));
            _regionManager.RegisterViewWithRegion("MainRegion", typeof(AnimeCardUserControl));

            // Controls Region
            _regionManager.RegisterViewWithRegion("ControlsRegion", typeof(CalendarUserControl));
            _regionManager.RegisterViewWithRegion("ControlsRegion", typeof(AboutUserControl));
            _regionManager.RegisterViewWithRegion("ControlsRegion", typeof(DownloadUserControl));
            _regionManager.RegisterViewWithRegion("ControlsRegion", typeof(KitsuAnimeCardUserControl));
            _regionManager.RegisterViewWithRegion("ControlsRegion", typeof(LatestEpisodesUserControl));
            _regionManager.RegisterViewWithRegion("ControlsRegion", typeof(SearchUserControl));
            _regionManager.RegisterViewWithRegion("ControlsRegion", typeof(SettingsUserControl));
            _regionManager.RegisterViewWithRegion("ControlsRegion", typeof(UpcomingAnimesUserControl));

            // Calendar Frame
            _regionManager.RegisterViewWithRegion("CalendarFrame", typeof(CalendarMenuUserControl));
            _regionManager.RegisterViewWithRegion("CalendarFrame", typeof(DayUserControl));

            // Download Frame
            _regionManager.RegisterViewWithRegion("DwnldFrame", typeof(QueueUserControl));
            _regionManager.RegisterViewWithRegion("DwnldFrame", typeof(HistoryUserControl));

            // Settings Frame
            _regionManager.RegisterViewWithRegion("SettingsFrame", typeof(DownloadSettingsUserControl));
            _regionManager.RegisterViewWithRegion("SettingsFrame", typeof(LogSettingsUserControl));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Windows
            containerRegistry.RegisterForNavigation<MainWindow>();

            // Controls
            containerRegistry.RegisterForNavigation<MenuUserControl>();
            containerRegistry.RegisterForNavigation<NavigatorUserControl>();
            containerRegistry.RegisterForNavigation<AboutUserControl>();
            containerRegistry.RegisterForNavigation<DownloadUserControl>();
            containerRegistry.RegisterForNavigation<LatestEpisodesUserControl>();
            containerRegistry.RegisterForNavigation<CalendarUserControl>();
            containerRegistry.RegisterForNavigation<SearchUserControl>();
            containerRegistry.RegisterForNavigation<SettingsUserControl>();
            containerRegistry.RegisterForNavigation<UpcomingAnimesUserControl>();
            containerRegistry.RegisterForNavigation<AnimeCardUserControl>();
            containerRegistry.RegisterForNavigation<KitsuAnimeCardUserControl>();

            // ------------------------- Sub Controls -------------------------

            // Settings Controls
            containerRegistry.RegisterForNavigation<DownloadSettingsUserControl>();
            containerRegistry.RegisterForNavigation<LogSettingsUserControl>();

            // Donwload Controls
            containerRegistry.RegisterForNavigation<QueueUserControl>();
            containerRegistry.RegisterForNavigation<HistoryUserControl>();

            // Calendar Controls
            containerRegistry.RegisterForNavigation<CalendarMenuUserControl>();
            containerRegistry.RegisterForNavigation<DayUserControl>();
        }
    }
}
