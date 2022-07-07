using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.Styling;
using FluentAvalonia.Core;
using FluentAvalonia.Core.ApplicationModel;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Controls;
using FluentAvalonia.UI.Media;
using FluentAvalonia.UI.Navigation;
using System;
using System.Runtime.InteropServices;
using Tengu.Services;
using Tengu.ViewModels;

namespace Tengu.Views
{
    public partial class MainWindow : CoreWindow
    {
        public static Window Instance { get; private set; }

        private MainWindowViewModel _viewModel;
        private NavigationView _navView;

        public MainWindow()
        {
            InitializeComponent();

            Instance = this;

            MinWidth = 1000;
            MinHeight = 600;

            Opened += OnWindowOpened;
            Closing += MainWindow_Closing;
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

        #region MICA and Theme management
        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);

            var thm = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
            thm.RequestedThemeChanged += OnRequestedThemeChanged;

            // Enable Mica on Windows 11
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // TODO: add Windows version to CoreWindow
                if (IsWindows11 && thm.RequestedTheme != FluentAvaloniaTheme.HighContrastModeString)
                {
                    TransparencyBackgroundFallback = Brushes.Transparent;
                    TransparencyLevelHint = WindowTransparencyLevel.Mica;

                    TryEnableMicaEffect(thm);
                }
            }

            thm.ForceWin32WindowToTheme(this);
        }

        private void OnRequestedThemeChanged(FluentAvaloniaTheme sender, RequestedThemeChangedEventArgs args)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                // TODO: add Windows version to CoreWindow
                if (IsWindows11 && args.NewTheme != FluentAvaloniaTheme.HighContrastModeString)
                {
                    TryEnableMicaEffect(sender);
                }
                else if (args.NewTheme == FluentAvaloniaTheme.HighContrastModeString)
                {
                    // Clear the local value here, and let the normal styles take over for HighContrast theme
                    SetValue(BackgroundProperty, AvaloniaProperty.UnsetValue);
                }
            }
        }

        private void TryEnableMicaEffect(FluentAvaloniaTheme thm)
        {

            // The background colors for the Mica brush are still based around SolidBackgroundFillColorBase resource
            // BUT since we can't control the actual Mica brush color, we have to use the window background to create
            // the same effect. However, we can't use SolidBackgroundFillColorBase directly since its opaque, and if
            // we set the opacity the color become lighter than we want. So we take the normal color, darken it and 
            // apply the opacity until we get the roughly the correct color
            // NOTE that the effect still doesn't look right, but it suffices. Ideally we need access to the Mica
            // CompositionBrush to properly change the color but I don't know if we can do that or not
            if (thm.RequestedTheme == FluentAvaloniaTheme.DarkModeString)
            {
                var color = this.TryFindResource("SolidBackgroundFillColorBase", out var value) ? (Color2)(Color)value : new Color2(32, 32, 32);

                color = color.LightenPercent(-0.8f);

                Background = new ImmutableSolidColorBrush(color, 0.78);
            }
            else if (thm.RequestedTheme == FluentAvaloniaTheme.LightModeString)
            {
                // Similar effect here
                var color = this.TryFindResource("SolidBackgroundFillColorBase", out var value) ? (Color2)(Color)value : new Color2(243, 243, 243);

                color = color.LightenPercent(0.5f);

                Background = new ImmutableSolidColorBrush(color, 0.9);
            }
        }
        #endregion
    }
}
