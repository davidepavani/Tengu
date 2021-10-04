using HandyControl.Tools;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.ViewModels
{
    public class SettingsUserControlViewModel : BindablePropertyBase
    {
        private readonly IRegionManager _regionManager;

        public SettingsUserControlViewModel(IRegionManager regionManager) : base()
        {
            _regionManager = regionManager;

            Navigate("DownloadSettingsUserControl");
        }
        public SettingsUserControlViewModel() { }

        public void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate("SettingsFrame", navigatePath);
            }
        }
    }
}
