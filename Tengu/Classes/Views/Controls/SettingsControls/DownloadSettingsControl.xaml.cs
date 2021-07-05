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
using Tengu.Classes.Views.Windows;

namespace Tengu.Classes.Views.Controls.SettingsControls
{
    /// <summary>
    /// Interaction logic for DownloadSettingsControl.xaml
    /// </summary>
    public partial class DownloadSettingsControl : UserControl
    {
        public DownloadSettingsControl()
        {
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            SettingsPage.settings_page.NavigateToLogSettingsPage();
        }

        private void btnDwnlDir_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog(MainWindow.main_window).GetValueOrDefault())
            {
                ProgramInfo.Instance.DownloadFolder = dialog.SelectedPath;
            }
        }
    }
}
