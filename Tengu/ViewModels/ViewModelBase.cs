using Avalonia;
using Avalonia.Media;
using FluentAvalonia.Styling;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Tengu.Business.API;
using Tengu.Business.API.Interfaces;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Objects;
using Tengu.Interfaces;

namespace Tengu.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        // Dependency Injection
        public INavigationService Navigator { get; private set; }
        public ITenguApi TenguApi { get; private set; }
        public IProgramConfiguration ProgramConfig { get; private set; }
        public IInfoBarService InfoBar { get; private set; }
        public IDownloadService DwnldService { get; private set; }

        // Commands
        public ICommand NavigateCommand { get; private set; }
        public ICommand NavigateBackCommand { get; private set; }

        // Properties
        public List<Hosts> HostsList { get; private set; }

        public ViewModelBase()
        {
            Hosts[] except = { Hosts.None };
            HostsList = Enum.GetValues(typeof(Hosts)).Cast<Hosts>().Except(except).ToList();

            // Dependency Injection
            Navigator = Locator.Current.GetService<INavigationService>();
            TenguApi = Locator.Current.GetService<ITenguApi>();
            ProgramConfig = Locator.Current.GetService<IProgramConfiguration>();
            InfoBar = Locator.Current.GetService<IInfoBarService>();
            DwnldService = Locator.Current.GetService<IDownloadService>();

            // Commands
            NavigateCommand = ReactiveCommand.Create<Type>(Navigate);
            NavigateBackCommand = ReactiveCommand.Create(NavigateBack);
        }

        public void RefreshTenguApiDownloadPath() 
        {
            TenguApi.DownloadPath = string.IsNullOrEmpty(ProgramConfig.Downloads.DownloadDirectory) ?
                        Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) :
                            ProgramConfig.Downloads.DownloadDirectory;
        }

        public void SetApplicationTheme() => AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>().RequestedTheme = ProgramConfig.Miscellaneous.IsDarkMode ? "Dark" : "Light";
        public void SetApplicationColor() => AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>().CustomAccentColor = Color.Parse(ProgramConfig.Miscellaneous.AppColor.Hex);

        private void Navigate(Type type)
        {
            if(type != null)
                Navigator.Navigate(type);
        }
        private void NavigateBack()
        {
            Navigator.GoBack();
        }
    }
}
