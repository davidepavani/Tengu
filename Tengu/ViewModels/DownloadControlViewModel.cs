using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class DownloadControlViewModel : ViewModelBase
    {
        private bool isPaneOpen = false;

        public ICommand OpenPaneCommand { get; set; }
        public ICommand CancelDownloadCommand { get; set; }
        public bool IsPaneOpen 
        { 
            get => isPaneOpen;
            set => this.RaiseAndSetIfChanged(ref isPaneOpen, value);
        }

        public DownloadControlViewModel()
        {
            OpenPaneCommand = ReactiveCommand.Create(() => IsPaneOpen = true);
            CancelDownloadCommand = ReactiveCommand.Create<DownloadModel>((dwn) =>
            {
                if(dwn != null)
                    DwnldService.CancelDownload(dwn);
            });
        }
    }
}
