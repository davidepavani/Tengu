using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Tengu.ViewModels;

namespace Tengu.Views
{
    public partial class HomePageControl : ReactiveUserControl<HomePageControlViewModel>
    {
        public HomePageControl()
        {
            InitializeComponent();
        }

        private void HlbNavigateLatest_Click(object sender, RoutedEventArgs args) 
        {
            ViewModel.Navigator.Navigate(typeof(LatestEpisodesControl));
        }
        private void HlbNavigateSearch_Click(object sender, RoutedEventArgs args)
        {
            ViewModel.Navigator.Navigate(typeof(SearchControl));
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
