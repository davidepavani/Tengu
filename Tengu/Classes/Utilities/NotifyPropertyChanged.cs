//using System;
//using System.Collections.Generic;
using System.ComponentModel;
//using System.Linq;
using System.Runtime.Serialization;
//using System.Text;
//using System.Threading.Tasks;

namespace Tengu.Classes.Utilities
{
    [DataContract]
    public class NotifyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
