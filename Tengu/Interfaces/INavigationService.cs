using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Interfaces
{
    public interface INavigationService
    {
        Frame NavigationFrame { get; }
        bool CanGoBack { get; }
        void SetFrame(Frame frame);
        void Navigate(Type t);
        void Navigate(Type t, object parameter);
        void GoBack();
    }
}
