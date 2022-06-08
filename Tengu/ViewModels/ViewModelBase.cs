using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Interfaces;

namespace Tengu.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        // Dependency Injection
        public INavigationService Navigator { get; private set; }
        public ITenguApi TenguApi { get; private set; }

        // Commands
        public ICommand NavigateCommand { get; private set; }
        public ICommand NavigateBackCommand { get; private set; }

        // Properties
        public List<Hosts> HostsList { get; private set; }

        public ViewModelBase()
        {
            Hosts[] except = { Hosts.None };
            HostsList = Enum.GetValues(typeof(Hosts)).Cast<Hosts>().Except(except).ToList();

            // Dependency Injection
            Navigator = Locator.Current.GetService<INavigationService>();
            TenguApi = Locator.Current.GetService<ITenguApi>();

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
