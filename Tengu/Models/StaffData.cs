using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Models
{
    public class StaffData : BindablePropertyBase
    {
        private string nick;
        private string role;

        #region Properties
        public string Nick
        {
            get { return nick; }
            set
            {
                if (nick != value) 
                {
                    nick = value;
                    RaisePropertyChanged();
                }
            }
        }
        public string Role
        {
            get { return role; }
            set
            {
                if (role != value)
                {
                    role = value;
                    RaisePropertyChanged();
                }
            }
        }
        #endregion
    }
}
