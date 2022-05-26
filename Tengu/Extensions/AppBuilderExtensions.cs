using Avalonia.Controls;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                        //ProgramConfiguration.Load(out ProgramConfiguration Configuration);
                        //Locator.CurrentMutable.RegisterConstant(Configuration, typeof(IProgramConfiguration));

                        // Navigation Service
                        Locator.CurrentMutable.RegisterConstant(new NavigationService(), typeof(INavigationService));
                    }
                });
            });
        }
    }
}
