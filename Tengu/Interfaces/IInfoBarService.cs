using Avalonia.Collections;
using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Models;

namespace Tengu.Interfaces
{
    public interface IInfoBarService
    {
        AvaloniaList<InfoModel> Messages { get; set; }
        void AddMessage(string title, string message, InfoBarSeverity severity);
        void RemoveMessage(InfoModel info);
    }
}
