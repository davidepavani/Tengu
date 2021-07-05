using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Classes.DataModels
{
    public class UsefulLinkData : BindablePropertyBase
    {
        private string name;
        private string link;
        private string image;

        #region Properties
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged();
            }
        }
        public string Link
        {
            get { return link; }
            set
            {
                link = value;
                RaisePropertyChanged();
            }
        }
        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
