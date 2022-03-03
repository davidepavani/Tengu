using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Tengu.Views
{
    public partial class CalendarPage : UserControl
    {
        public CalendarPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
