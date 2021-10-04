using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Shared.Utilities;
using System.ComponentModel;

namespace Tengu.Models
{
    public class DictionaryData : NotifyPropertyChanged
    {
        private string key;
        private string value;

        #region Properties
        public string Key
        {
            get { return key; }
            set
            {
                key = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Key"));
            }
        }
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Value"));
            }
        }
        #endregion
    }
}
