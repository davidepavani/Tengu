using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tengu.ViewModels
{
    public class SettingsPageViewModel
    {
        public string DefaultFolder { get; private set; }

        public SettingsPageViewModel()
        {
            DefaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);

        }
    }
}
