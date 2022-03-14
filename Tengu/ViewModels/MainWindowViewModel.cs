using Avalonia;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Windows.Input;
using Tengu.Interfaces;

namespace Tengu.ViewModels
{
    public class MainWindowViewModel : ReactiveObject
    {
        private readonly IProgramConfiguration Configuration;
        public MainWindowViewModel()
        {
            Configuration = Locator.Current.GetService<IProgramConfiguration>();
        }

        public void SaveConfiguration()
        {
            Configuration.Save();
        }
    }
}