using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using NLog;
using Notification.Wpf;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Models;
using static Tengu.Utilities.PrismEvents;
using Logger = NLog.Logger;

namespace Tengu.ViewModels
{
    public class DownloadUserControlViewModel : BindablePropertyBase
    {
        private readonly IRegionManager _regionManager;

        public DownloadUserControlViewModel() { }
        public DownloadUserControlViewModel(IRegionManager regionManager) : base()
        {
            _regionManager = regionManager;

            Navigate("QueueUserControl");
        }

        public void Navigate(string navigatePath)
        {
            if (navigatePath != null)
            {
                _regionManager.RequestNavigate("DwnldFrame", navigatePath);
            }
        }
    }
}
