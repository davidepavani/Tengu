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

namespace Tengu.Views.Controls.SettingsControls
{
    /// <summary>
    /// Interaction logic for DownloadSettingsControl.xaml
    /// </summary>
    public partial class DownloadSettingsUserControl : UserControl
    {
        public DownloadSettingsUserControl()
        {
            InitializeComponent();
        }

        private void btnDwnlDir_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            dialog.ShowNewFolderButton = true;

            if (dialog.ShowDialog().GetValueOrDefault())
            {
                ProgramInfos.Instance.DownloadFolder = dialog.SelectedPath;
            }
        }
    }
}
