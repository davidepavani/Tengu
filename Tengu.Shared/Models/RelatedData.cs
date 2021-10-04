using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Shared.Utilities;
using System.ComponentModel;

namespace Tengu.Models
{
    public class RelatedData : NotifyPropertyChanged
    {
        private string title;
        private string link;
        private string image;

        #region Properties
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
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
        public string Link
        {
            get { return link; }
            set
            {
                link = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Link"));
            }
        }
        #endregion
    }
}
