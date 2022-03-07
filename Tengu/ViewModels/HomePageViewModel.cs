using Avalonia;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;

namespace Tengu.ViewModels
{
    public class HomePageViewModel : ReactiveObject
    {
        private readonly MaterialTheme MaterialThemeStyles =
            Application.Current!.LocateMaterialTheme<MaterialTheme>();

        public string Description { get; private set; }
        public ICommand OpenProjectLinkCommand { get; private set; }
        public ICommand SetDarkModeCommand { get; private set; }
        public ICommand SetLightModeCommand { get; private set; }

        public HomePageViewModel()
        {
            SetDarkModeCommand = ReactiveCommand.Create(UseMaterialUIDarkTheme);
            SetLightModeCommand = ReactiveCommand.Create(UseMaterialUILightTheme);

            OpenProjectLinkCommand = ReactiveCommand.Create<string>(OpenGitHubProject);

            Description = "Easy, Intuitive and cross-platform Application for everything related to Animes.\n";
            Description += "Download Episodes, search, and stay up to date!\n";
            Description += "With a solid (and definitely not broken) Backend <3\n";
            Description += "Developed by two poor people with no experience.";
        }

        public void UseMaterialUIDarkTheme()
            => MaterialThemeStyles.BaseTheme = BaseThemeMode.Dark;

        public void UseMaterialUILightTheme()
            => MaterialThemeStyles.BaseTheme = BaseThemeMode.Light;

        public void OpenGitHubProject(string project)
        {
            string url = project switch
            {
                "Tengu.Business" => "https://github.com/giuseppeSalerno10/Tengu.Business",
                _ => "https://github.com/Dugongoo/Tengu",
            };

            try
            {
                Process.Start(url);
            }
            catch
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);
                }
                else
                {
                    // Logging
                }
            }
        }
    }
}
