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
using System.Windows.Input;
using Avalonia.Controls;

namespace Tengu.ViewModels
{
    public class SettingsControlViewModel : ViewModelBase
    {
        private string downloadDirectory;
        private CustomColorModel selectedColor;
        private bool isDarkMode;

        public string DefaultDownloadPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
        public List<CustomColorModel> DefaultColors { get; private set; }
        public string DownloadDirectory
        {
            get => downloadDirectory;
            set => this.RaiseAndSetIfChanged(ref downloadDirectory, value);
        }
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

            DownloadDirectory = ProgramConfig.Downloads.DownloadDirectory;
        }

        private void ChangeTheme()
        {
            ProgramConfig.Miscellaneous.IsDarkMode = IsDarkMode;
            SetApplicationTheme();
        }

        public void SelectFolder(string path)
        {
            DownloadDirectory = path;
            ProgramConfig.Downloads.DownloadDirectory = path;

            RefreshTenguApiDownloadPath();
        }

        private void ChangeColor()
        {
            ProgramConfig.Miscellaneous.AppColor = SelectedColor;
            SetApplicationColor();
        }
    }
}
