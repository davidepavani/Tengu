using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Downla;
using Microsoft.Extensions.Logging;
using NLog;
using Splat;
using System;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Business.Core;

namespace Tengu
{
    public class myclass
    {
        private readonly Microsoft.Extensions.Logging.ILogger<TenguApi> Logger;
        private readonly Microsoft.Extensions.Logging.ILogger ExtraLogger;

        public myclass(ILogger<TenguApi> logger, ILoggerFactory loggerFactory)
        {
            Logger = logger;
            ExtraLogger = loggerFactory.CreateLogger<TenguApi>();
        }
    }

    internal class Program
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        [STAThread]
        public static void Main(string[] args) => BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);

        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
        {
            // Downla
            FilesService fileService = new();
            HttpConnectionService httpConnectionService = new();
            MimeMapperService mimeMapperService = new();

            DownlaClient downlaClient = new(httpConnectionService, mimeMapperService, fileService);

            Locator.CurrentMutable.RegisterConstant(fileService, typeof(IFilesService));
            Locator.CurrentMutable.RegisterConstant(httpConnectionService, typeof(IHttpConnectionService));
            Locator.CurrentMutable.RegisterConstant(mimeMapperService, typeof(IMimeMapperService));

            Locator.CurrentMutable.RegisterConstant(downlaClient, typeof(IDownlaClient));

            // Utilities
            AnimeSaturnUtilities saturnUtilities = new();
            AnimeUnityUtilities unityUtilities = new();

            Locator.CurrentMutable.RegisterConstant(saturnUtilities, typeof(IAnimeSaturnUtilities)); 
            Locator.CurrentMutable.RegisterConstant(unityUtilities, typeof(IAnimeUnityUtilities));

            // Adapters
            AnimeSaturnAdapter saturnAdapter = new(saturnUtilities);
            AnimeUnityAdapter unityAdapter = new(unityUtilities);
            KitsuAdapter kitsuAdapter = new();

            Locator.CurrentMutable.RegisterConstant(saturnAdapter, typeof(IAnimeSaturnAdapter));
            Locator.CurrentMutable.RegisterConstant(unityAdapter, typeof(IAnimeUnityAdapter));
            Locator.CurrentMutable.RegisterConstant(kitsuAdapter, typeof(IKitsuAdapter));

            // Managers
            AnimeSaturnManager saturnManager = new(saturnAdapter, saturnUtilities, downlaClient);
            AnimeUnityManager unityManager = new(unityAdapter, unityUtilities, downlaClient);
            KitsuManager kitsuManager = new(kitsuAdapter);

            Locator.CurrentMutable.RegisterConstant(saturnManager, typeof(IAnimeSaturnManager));
            Locator.CurrentMutable.RegisterConstant(unityManager, typeof(IAnimeUnityManager));
            Locator.CurrentMutable.RegisterConstant(kitsuManager, typeof(IKitsuManager));

            var loggerFactory = LoggerFactory.Create(builder => builder.Configure((ad) => { }));
            Microsoft.Extensions.Logging.ILogger<TenguApi> logger = loggerFactory.CreateLogger<TenguApi>();
            logger.LogInformation("Starting...");

            TenguApi tenguApi = new(unityManager, saturnManager, kitsuManager, logger);
            tenguApi.CurrentHosts = new Hosts[] { Hosts.AnimeUnity, Hosts.AnimeSaturn };

            Locator.CurrentMutable.RegisterConstant(tenguApi, typeof(ITenguApi));

            return AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace()
                .UseReactiveUI();

            
        }
    }
}
