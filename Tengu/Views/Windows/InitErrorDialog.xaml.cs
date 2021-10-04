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
using System.Windows.Shapes;
using HandyControl.Tools;

namespace Tengu.Views.Windows
{
    /// <summary>
    /// Interaction logic for CustomDialog.xaml
    /// </summary>
    public partial class InitErrorDialog
    {
        public InitErrorDialog(string message)
        {
            InitializeComponent();

            txtMessage.Text = message;

            // Bring to front
            this.Activate();
            this.Topmost = true;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (Exception)
            {
                // Security..
            }
        }

        private void btnRetry_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
