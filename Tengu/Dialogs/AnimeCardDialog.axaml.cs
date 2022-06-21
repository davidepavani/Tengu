using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tengu.Dialogs
{
    public partial class AnimeCardDialog : UserControl
    {
        public AnimeCardDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
