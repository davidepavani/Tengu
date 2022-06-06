using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tengu.Views
{
    public partial class CalendarControl : UserControl
    {
        public CalendarControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
