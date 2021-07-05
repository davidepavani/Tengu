using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Tengu
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SplashWindow.Init(() => {
                Classes.Views.Windows.SplashWindow splash = 
                            new Classes.Views.Windows.SplashWindow();
                return splash;
            });
            base.OnStartup(e);
        }
    }
}
