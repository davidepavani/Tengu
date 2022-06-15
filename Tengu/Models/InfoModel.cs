using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Models
{
    public class InfoModel
    {
        public string Title { get; private set; }
        public string Message { get; private set; }
        public InfoBarSeverity Severity { get; private set; }
        public bool IsOpen { get; private set; }

        public InfoModel(string title, string message, InfoBarSeverity severity)
        {
            Title = title;
            Message = message;
            Severity = severity;
            IsOpen = true;
        }
    }
}
