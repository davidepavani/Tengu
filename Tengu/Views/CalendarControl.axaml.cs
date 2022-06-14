using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Tengu.ViewModels;

namespace Tengu.Views
{
    public partial class CalendarControl : ReactiveUserControl<CalendarControlViewModel>
    {
        public CalendarControl()
        {
            InitializeComponent();

            Initialized += CalendarControl_Initialized;
        }

        private void CalendarControl_Initialized(object sender, System.EventArgs e)
        {
            ViewModel.Initialize();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
