using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Tengu.ViewModels;

namespace Tengu.Views
{
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public static Window WindowInstance { get; private set; }

        #region Control fields
        private ListBox DrawerList;
        private Carousel PageCarousel;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            WindowInstance = this;

            Closing += MainWindow_Closing;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ViewModel.SaveConfiguration();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);

            #region Control getter and event binding
            DrawerList = this.Get<ListBox>(nameof(DrawerList));
            DrawerList.PointerReleased += DrawerSelectionChanged;
            DrawerList.KeyUp += DrawerList_KeyUp;

            PageCarousel = this.Get<Carousel>(nameof(PageCarousel));
            #endregion
        }

        private void DrawerList_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.Enter)
                DrawerSelectionChanged(sender, null);
        }

        public void DrawerSelectionChanged(object sender, RoutedEventArgs args)
        {
            var listBox = sender as ListBox;
            if (!listBox.IsFocused && !listBox.IsKeyboardFocusWithin)
                return;
            try
            {
                PageCarousel.SelectedIndex = listBox.SelectedIndex;

            }
            catch
            {
            }
        }

    }
}
