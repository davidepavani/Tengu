using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Tengu.ViewModels;

namespace Tengu.Views
{
    public partial class SettingsControl : ReactiveUserControl<SettingsControlViewModel>
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        private async void SelectFolder_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new()
            {
                Title = "Choose Download Directory"
            };

            var result = await dialog.ShowAsync(MainWindow.Instance);
            if (result != null)
                ViewModel.SelectFolder(result);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
