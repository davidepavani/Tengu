using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tengu.ViewModels
{
    public class DownloadControlViewModel : ViewModelBase
    {
        private bool isPaneOpen = false;

        public ICommand OpenPaneCommand { get; set; }
        public bool IsPaneOpen 
        { 
            get => isPaneOpen;
            set => this.RaiseAndSetIfChanged(ref isPaneOpen, value);
        }

        public List<string> Downloads { get; set; } = new()
        {
            "downloadaaaaaaaaaaaaaaaaaaaaaaaaa 1",
            "download 2",
            "download 3",
        };
        public string CurrentDownload { get; set; } = "Title";

        public DownloadControlViewModel()
        {
            OpenPaneCommand = ReactiveCommand.Create(() => IsPaneOpen = true);
        }
    }
}
