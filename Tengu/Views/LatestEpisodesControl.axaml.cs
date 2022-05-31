using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Tengu.ViewModels;

namespace Tengu.Views
{
    public partial class LatestEpisodesControl : ReactiveUserControl<LatestEpisodesControlViewModel>
    {
        public LatestEpisodesControl()
        {
            InitializeComponent();

            Initialized += LatestEpisodesControl_Initialized;
        }

        private void LatestEpisodesControl_Initialized(object sender, System.EventArgs e)
        {
            ViewModel.Initialize();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
