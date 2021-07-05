using HandyControl.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Tengu.Classes.DataModels;
using Tengu.Classes.Views.Windows;

namespace Tengu.Classes.Views.Controls.SearchControls
{
    /// <summary>
    /// Interaction logic for DialogSearchedAnime.xaml
    /// </summary>
    public partial class DialogSearchedAnime : Border
    {
        public DialogSearchedAnime(AnimeData data)
        {
            InitializeComponent();
            this.DataContext = data;
        }

        private void btnShowAnimeCard_Click(object sender, RoutedEventArgs e)
        {
            AnimeData data = this.DataContext as AnimeData;
            MainWindow.main_window.NavigateToAnimeCardPage(data.LinkCard);
        }
    }
}
