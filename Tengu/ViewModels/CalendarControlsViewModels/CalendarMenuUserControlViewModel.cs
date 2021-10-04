using HandyControl.Tools;
using HandyControl.Tools.Command;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Enums;
using Tengu.Shared.Enums;

namespace Tengu.ViewModels.CalendarControlsViewModels
{
    public class CalendarMenuUserControlViewModel : BindablePropertyBase
    {
        private readonly IRegionManager _regionManager;
        public ICommand NavigateCommand { get; private set; }

        public CalendarMenuUserControlViewModel(IRegionManager regionManager) : base()
        {
            _regionManager = regionManager;

            NavigateCommand = new RelayCommand<string>(Navigate);
        }
        public CalendarMenuUserControlViewModel() { }

        public void Navigate(string day)
        {
            if (day != null)
            {
                // Define navigation parameters
                NavigationParameters navigationParams = new();
                navigationParams.Add("Day", day.ParseEnum<CalendarDay>());

                _regionManager.RequestNavigate("CalendarFrame", "DayUserControl", navigationParams);
            }
        }
    }
}
