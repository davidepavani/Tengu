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
using Tengu.Classes.ViewModels;
using Tengu.Classes.Enums;

namespace Tengu.Classes.Views.Controls.CalendarControls
{
    /// <summary>
    /// Interaction logic for DayControl.xaml
    /// </summary>
    public partial class DayControl : UserControl
    {
        public DayControl(CalendarDay day)
        {
            InitializeComponent();
            DataContext = new CalendarViewModel(day);
        }
    }
}
