using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Classes.DataModels
{
    public class DictionaryData : BindablePropertyBase
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
                RaisePropertyChanged();
            }
        }
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
