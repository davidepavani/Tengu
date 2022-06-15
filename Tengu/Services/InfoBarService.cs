using Avalonia.Collections;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Interfaces;
using Tengu.Models;

namespace Tengu.Services
{
    public class InfoBarService : ReactiveObject, IInfoBarService
    {
        private readonly object syncInfo = new();
        private AvaloniaList<InfoModel> messages = new();

        public AvaloniaList<InfoModel> Messages
        {
            get => messages;
            set => this.RaiseAndSetIfChanged(ref messages, value);
        }

        public void AddMessage(string title, string message, InfoBarSeverity severity)
        {
            lock (syncInfo)
            {
                Messages.Add(new(title, message, severity));
            }
        }

        public void RemoveMessage(InfoModel info)
        {
            lock(syncInfo) 
            {
                Messages.Remove(info);
            }
        }
    }
}
