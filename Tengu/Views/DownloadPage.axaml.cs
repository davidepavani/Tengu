using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Tengu.ViewModels;

namespace Tengu.Views
{
    public partial class DownloadPage : ReactiveUserControl<DownloadPageViewModel>
    {
        public DownloadPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
