using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Shared.Utilities;
using System.ComponentModel;

namespace Tengu.Models
{
    public class UsefulLinkData : NotifyPropertyChanged
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
                OnPropertyChanged(new PropertyChangedEventArgs("Name"));
            }
        }
        public string Link
        {
            get { return link; }
            set
            {
                link = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Link"));
            }
        }
        public string Image
        {
            get { return image; }
            set
            {
                image = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Image"));
            }
        }
        #endregion
    }
}
