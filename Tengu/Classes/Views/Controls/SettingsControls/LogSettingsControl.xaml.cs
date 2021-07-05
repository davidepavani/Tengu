using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tengu.Classes.Views.Controls.SettingsControls
{
    /// <summary>
    /// Interaction logic for LogSettingsControl.xaml
    /// </summary>
    public partial class LogSettingsControl : UserControl
    {
        public LogSettingsControl()
        {
            InitializeComponent();
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage.settings_page.NavigateToDownloadSettingsPage();
        }
    }
}
