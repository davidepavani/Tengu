using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Classes.DataModels
{
    public class StaffData : BindablePropertyBase
    {
        private string nick;
        private string role;

        public string Nick
        {
            get { return nick; }
            set
            {
                nick = value;
                RaisePropertyChanged();
            }
        }
        public string Role
        {
            get { return role; }
            set
            {
                role = value;
                RaisePropertyChanged();
            }
        }
    }
}
