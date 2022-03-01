using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tengu.Views
{
    public partial class SearchPage : UserControl
    {
        public SearchPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
