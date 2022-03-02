using Avalonia;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;

namespace Tengu.ViewModels
{
    public class HomePageViewModel : ReactiveObject
    {
        private readonly MaterialTheme MaterialThemeStyles =
            Application.Current!.LocateMaterialTheme<MaterialTheme>();

        public ICommand OpenProjectLinkCommand { get; set; }
        public ICommand SetDarkModeCommand { get; set; }
        public ICommand SetLightModeCommand { get; set; }


        public HomePageViewModel()
        {
            SetDarkModeCommand = ReactiveCommand.Create(UseMaterialUIDarkTheme);
            SetLightModeCommand = ReactiveCommand.Create(UseMaterialUILightTheme);

            OpenProjectLinkCommand = ReactiveCommand.Create(OpenGitHubProject);
        }

        public void UseMaterialUIDarkTheme()
            => MaterialThemeStyles.BaseTheme = BaseThemeMode.Dark;

        public void UseMaterialUILightTheme()
            => MaterialThemeStyles.BaseTheme = BaseThemeMode.Light;

        public void OpenGitHubProject()
            => Process.Start("https://github.com/Dugongoo/Tengu");
    }
}
