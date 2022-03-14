using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Microsoft.Extensions.Logging;
using NLog;
using Splat;
using System;
using Tengu.Configuration;
using Tengu.Downloads;
using Tengu.Interfaces;
using Tengu.Utilities;
using Tengu.Views;

namespace Tengu
{
    internal class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            ProgramConfiguration.Load(out ProgramConfiguration configuration);
            Locator.CurrentMutable.RegisterConstant(configuration, typeof(IProgramConfiguration));

            Locator.CurrentMutable.RegisterConstant(new DownloadManager(), typeof(IDownloadManager));

            return AppBuilder.Configure<App>()
                .UseTenguApi()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();
        }
    }
}
