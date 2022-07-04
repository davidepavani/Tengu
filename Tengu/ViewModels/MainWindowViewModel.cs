using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ReactiveUI;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ICommand CloseInfoCommand { get; set; }
        public MainWindowViewModel()
        {
            CloseInfoCommand = ReactiveCommand.Create<InfoModel>(CloseInfoMessage);
            RefreshTenguApiDownloadPath();
        }

        public void CloseInfoMessage(InfoModel info)
        {
            InfoBar.RemoveMessage(info);
        }

        public void Close()
        {
            ProgramConfig.Save();
        }
    }
}
