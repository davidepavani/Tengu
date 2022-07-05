using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Data;
using Tengu.Models;
using ReactiveUI;
using Avalonia;
using FluentAvalonia.Styling;
using Avalonia.Media;

namespace Tengu.ViewModels
{
    public class SettingsControlViewModel : ViewModelBase
    {
        private CustomColorModel selectedColor;
        private bool isDarkMode;

        public List<CustomColorModel> DefaultColors { get; private set; }
        public bool IsDarkMode
        {
            get => isDarkMode;
            set
            {
                this.RaiseAndSetIfChanged(ref isDarkMode, value);
                ChangeTheme();
            }
        }
        public CustomColorModel SelectedColor
        {
            get => selectedColor;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedColor, value);
                ChangeColor();
            }
        }

        public SettingsControlViewModel()
        {
            DefaultColors = Misc.LoadCustomDefaultColors().ToList();
            SelectedColor = DefaultColors.SingleOrDefault(x => x.Hex == ProgramConfig.Miscellaneous.AppColor.Hex);
            IsDarkMode = ProgramConfig.Miscellaneous.IsDarkMode;
        }

        private void ChangeTheme()
        {
            ProgramConfig.Miscellaneous.IsDarkMode = IsDarkMode;
            SetApplicationTheme();
        }

        private void ChangeColor()
        {
            ProgramConfig.Miscellaneous.AppColor = SelectedColor;
            SetApplicationColor();
        }
    }
}
