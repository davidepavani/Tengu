using Avalonia.Controls;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Business.API;
using Tengu.Interfaces;
using Tengu.Views;

namespace Tengu.ViewModels
{
    public class SettingsPageViewModel : ReactiveObject
    {
        private readonly IProgramConfiguration configuration;
        private readonly ITenguApi tenguApi;
        public IDownloadManager DownloadManager { get; private set; }

        private string folderPath;

        public ICommand SelectFolderCommand { get; private set; }
        public string DefaultFolder { get; private set; }
        public string FolderPath
        {
            get => folderPath;
            set => this.RaiseAndSetIfChanged(ref folderPath, value);
        }

        public SettingsPageViewModel()
        {
            DefaultFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
            
            configuration = Locator.Current.GetService<IProgramConfiguration>();
            DownloadManager = Locator.Current.GetService<IDownloadManager>();
            tenguApi = Locator.Current.GetService<ITenguApi>();

            FolderPath = configuration.DownloadDirectory;

            SelectFolderCommand = ReactiveCommand.Create(SelectFolder);
        }

        public async void SelectFolder()
        {
            OpenFolderDialog dialog = new();
            dialog.Title = "Select Destination Download folder";

            string result = await dialog.ShowAsync(MainWindow.WindowInstance);

            if (!string.IsNullOrWhiteSpace(result))
            {
                FolderPath = result;
                configuration.DownloadDirectory = result;
                tenguApi.DownloadPath = result;
            }
        }
    }
}
