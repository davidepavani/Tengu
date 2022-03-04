using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Threading.Tasks;
using Tengu.ViewModels;

namespace Tengu.Views
{
    public partial class LatestEpisodesPage : ReactiveUserControl<LatestEpisodesPageViewModel>
    {
        public LatestEpisodesPage()
        {
            InitializeComponent();

            this.WhenActivated(d => d(ViewModel!.ShowAnimeModelDialog.RegisterHandler(DoShowAnimeModelDialogAsync)));
        }

        private async Task DoShowAnimeModelDialogAsync(InteractionContext<AnimeModelDialogViewModel, object> interaction)
        {
            AnimeModelDialog dialog = new()
            {
                DataContext = interaction.Input
            };

            var result = await dialog.ShowDialog<AnimeModelDialogViewModel>(MainWindow.WindowInstance);
            interaction.SetOutput(result);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
