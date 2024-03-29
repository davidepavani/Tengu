﻿using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API;
using Tengu.Business.API.Interfaces;
using Tengu.Configuration;
using Tengu.Interfaces;
using Tengu.Services;

namespace Tengu.Extensions
{
    public static class AppBuilderExtensions
    {
        public static TAppBuilder TenguInjection<TAppBuilder>(this TAppBuilder builder) where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            return builder.AfterPlatformServicesSetup(delegate
            {
                Locator.RegisterResolverCallbackChanged(delegate
                {
                    if (Locator.CurrentMutable != null)
                    {
                        // Program Configuration
                        ProgramConfiguration.Load(out ProgramConfiguration Configuration);
                        Locator.CurrentMutable.RegisterConstant(Configuration, typeof(IProgramConfiguration));

                        // Navigation Service
                        Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(INavigationService));

                        // InfoBar Service
                        Locator.CurrentMutable.RegisterConstant(new InfoBarService(), typeof(IInfoBarService));

                        // Download Service
                        Locator.CurrentMutable.RegisterConstant(new DownloadService(), typeof(IDownloadService));
                    }
                });
            });
        }

        public static TAppBuilder UseTenguApi<TAppBuilder>(this TAppBuilder builder) where TAppBuilder : AppBuilderBase<TAppBuilder>, new()
        {
            return builder.AfterPlatformServicesSetup(delegate
            {
                Locator.RegisterResolverCallbackChanged(delegate
                {
                    if (Locator.CurrentMutable != null)
                    {
                        using IHost host = Host.CreateDefaultBuilder()
                                 .ConfigureServices((_, services) =>
                                     services.AddTenguServices()
                                      .AddLogging(loggingBuilder => 
                                          {                           
                                              loggingBuilder.ClearProviders();              
                                              loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);             
                                              loggingBuilder.AddNLog();          
                                          }) 
                                      )
                                 .Build();

                        TenguApi tenguApi = (TenguApi)ActivatorUtilities.CreateInstance(host.Services, typeof(TenguApi));

                        Locator.CurrentMutable.RegisterConstant(tenguApi, typeof(ITenguApi));
                    }
                });
            });
        }
    }
}
