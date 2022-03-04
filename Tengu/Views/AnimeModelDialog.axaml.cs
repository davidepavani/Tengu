using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Tengu.ViewModels;

namespace Tengu.Views
{
    public partial class AnimeModelDialog : ReactiveWindow<AnimeModelDialogViewModel>
    {
        public AnimeModelDialog()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
