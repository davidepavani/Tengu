using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Data;
using Tengu.Models;
using ReactiveUI;

namespace Tengu.ViewModels
{
    public class SettingsControlViewModel : ViewModelBase
    {
        private CustomColorModel selectedColor;

        public List<CustomColorModel> DefaultColors { get; private set; }
        public CustomColorModel SelectedColor
        {
            get => selectedColor;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedColor, value);

                // CHANGE COLOR - TODO
            }
        }

        public SettingsControlViewModel()
        {
            DefaultColors = Misc.LoadCustomDefaultColors().ToList();
            SelectedColor = ProgramConfig.Miscellaneous.AppColor;
        }
    }
}
