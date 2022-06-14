using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Styling;
using FluentAvalonia.Core;
using FluentAvalonia.Core.ApplicationModel;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Navigation;
using System;
using Tengu.Services;
using Tengu.ViewModels;

namespace Tengu.Views
{
    public partial class MainWindow : CoreWindow
    {
        private MainWindowViewModel _viewModel;
        private NavigationView _navView;

        public MainWindow()
        {
            InitializeComponent();

            MinWidth = 800;
            MinHeight = 600;

            Opened += OnWindowOpened;
            Closing += MainWindow_Closing;

            // TODO => REMOVE THIS
            var faTheme = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
            faTheme.RequestedTheme = "Dark";
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            (DataContext as MainWindowViewModel).Close();
        }

        private void OnWindowOpened(object sender, EventArgs e)
        {
            TitleBar.ExtendViewIntoTitleBar = true;

            if (this.FindControl<Grid>("TitleBarHost") is Grid g)
            {
                SetTitleBar(g);
            }

            //
            _viewModel = DataContext as MainWindowViewModel;
            _navView = this.FindControl<NavigationView>("NavView");

            _viewModel.Navigator.SetFrame(this.FindControl<Frame>("FrameView"));
            _viewModel.Navigator.NavigationFrame.Navigated += NavigationFrame_Navigated;

            _navView.ItemInvoked += OnNavigationViewItemInvoked;
            _navView.BackRequested += OnNavigationViewBackRequested;

            _viewModel.Navigator.Navigate(typeof(HomePageControl));
        }

        private void OnNavigationViewItemInvoked(object sender, NavigationViewItemInvokedEventArgs e)
        {
            if (e.InvokedItemContainer is NavigationViewItem nvi && nvi.Tag is Type typ)
            {
                _viewModel.Navigator.Navigate(typ);
            }
        }
        private void OnNavigationViewBackRequested(object sender, NavigationViewBackRequestedEventArgs e)
        {
            _viewModel.Navigator.GoBack();
        }

        private void NavigationFrame_Navigated(object sender, NavigationEventArgs e)
        {
            bool found = false;

            foreach (NavigationViewItem nvi in _navView.MenuItems)
            {
                if (nvi.Tag is Type tag && tag == e.SourcePageType)
                {
                    found = true;
                    _navView.SelectedItem = nvi;
                    break;
                }
            }

            if (!found)
            {
                // only remaining page type is core controls pages
                _navView.SelectedItem = _navView.MenuItems.ElementAt(1);
            }
        }
    }
}
