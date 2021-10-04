using HandyControl.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.Models
{
    public class HistoryData : BindablePropertyBase
    {
        #region Declarations
        private string title;
        private string episode;

        private string error_message;
        private bool in_error;
        #endregion

        #region Properties
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged();
            }
        }
        public string Episode
        {
            get { return episode; }
            set
            {
                episode = value;
                RaisePropertyChanged();
            }
        }
        public bool InError
        {
            get { return in_error; }
            set
            {
                in_error = value;
                RaisePropertyChanged();
            }
        }
        public string ErrorMessage
        {
            get { return error_message; }
            set
            {
                error_message = value;
                RaisePropertyChanged();
            }
        }
        #endregion
    }
}
