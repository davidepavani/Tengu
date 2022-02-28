using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tengu.Views
{
    public partial class UpcomingPage : UserControl
    {
        public UpcomingPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
