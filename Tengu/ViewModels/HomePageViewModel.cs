using Avalonia;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using NLog;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Input;
using Tengu.Logging;

namespace Tengu.ViewModels
{
    public class HomePageViewModel : ReactiveObject
    {
        private readonly Logger log = LogManager.GetLogger(Loggers.HomeLoggerName);

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
        {
            log.Trace("Setted DarkMode");
            MaterialThemeStyles.BaseTheme = BaseThemeMode.Dark;
        }

        public void UseMaterialUILightTheme()
        {
            log.Trace("Setted LightMode");
            MaterialThemeStyles.BaseTheme = BaseThemeMode.Light;

            
        }

        public void OpenGitHubProject(string project)
        {
            string url = project switch
            {
                "Tengu.Business" => "https://github.com/giuseppeSalerno10/Tengu.Business",
                _ => "https://github.com/Dugongoo/Tengu",
            };

            log.Info($"Opening {project} GitHub Project link: {url}");

            try
            {
                Process.Start(url);
            }
            catch(Exception ex)
            {
                // hack because of this: https://github.com/dotnet/corefx/issues/10361
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    url = url.Replace("&", "^&");
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true });

                    log.Info($"Opened Windows platform");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);

                    log.Info($"Opened Linux platform");
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url);

                    log.Info($"Opened OSX platform");
                }
                else
                {
                    log.Error(ex, $"Cannot open {project} GitHub Project link: {url}");
                }
            }
        }
    }
}
