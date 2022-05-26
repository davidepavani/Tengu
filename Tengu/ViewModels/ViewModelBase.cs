using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Tengu.Interfaces;

namespace Tengu.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        // Dependency Injection
        public INavigationService Navigator { get; private set; }

        // Commands
        public ICommand NavigateCommand { get; private set; }
        public ICommand NavigateBackCommand { get; private set; }

        public ViewModelBase()
        {
            // Dependency Injection
            Navigator = Locator.Current.GetService<INavigationService>();

            // Commands
            NavigateCommand = ReactiveCommand.Create<Type>(Navigate);
            NavigateBackCommand = ReactiveCommand.Create(NavigateBack);
        }

        private void Navigate(Type type)
        {
            if(type != null)
                Navigator.Navigate(type);
        }
        private void NavigateBack()
        {
            Navigator.GoBack();
        }
    }
}
