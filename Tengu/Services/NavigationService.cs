using FluentAvalonia.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Interfaces;

namespace Tengu.Services
{
    public class NavigationService : INavigationService
    {
        public Frame NavigationFrame { get; private set; }
        public bool CanGoBack { get => NavigationFrame != null && NavigationFrame.CanGoBack; }

        public void SetFrame(Frame frame)
        {
            NavigationFrame = frame;
        }

        public void Navigate(Type t)
        {
            if (t != null)
                NavigationFrame.Navigate(t);
        }

        public void Navigate(Type t, object parameter)
        {
            if (t != null && parameter != null)
                NavigationFrame.Navigate(t, parameter);
        }

        public void GoBack()
        {
            if (CanGoBack)
            {
                NavigationFrame.GoBack();
            }
        }
    }
}
