using Avalonia;
using Avalonia.Controls;
using FluentAvalonia.Core.ApplicationModel;
using FluentAvalonia.Styling;
using FluentAvalonia.UI.Controls;
using System;

namespace Tengu.Views
{
    public partial class MainWindow : CoreWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            MinWidth = 600;
            MinHeight = 500;

            Opened += OnParentWindowOpened;

            var faTheme = AvaloniaLocator.Current.GetService<FluentAvaloniaTheme>();
            faTheme.RequestedTheme = "Dark";
        }

        private void OnParentWindowOpened(object sender, EventArgs e)
        {
            var titleBar = this.TitleBar;
            if (titleBar != null)
            {
                titleBar.ExtendViewIntoTitleBar = true;

                titleBar.LayoutMetricsChanged += OnApplicationTitleBarLayoutMetricsChanged;

                if (this.FindControl<Grid>("TitleBarHost") is Grid g)
                {
                    SetTitleBar(g);
                    g.Margin = new Thickness(0, 0, titleBar.SystemOverlayRightInset, 0);
                }
            }
        }

        private void OnApplicationTitleBarLayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args)
        {
            if (this.FindControl<Grid>("TitleBarHost") is Grid g)
            {
                g.Margin = new Thickness(0, 0, sender.SystemOverlayRightInset, 0);
            }
        }
    }
}
