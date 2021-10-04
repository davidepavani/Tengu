
using System;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Windows.Input;
using System.Globalization;
using System.Text.RegularExpressions;
using Tengu.Shared.Utilities;
using System.ComponentModel;

namespace Tengu.Models
{
    public partial class Anime : NotifyPropertyChanged
    {
        #region Declarations
        private string title;
        private string episode;
        private string image_poster;
        private string image_cover;
        private string link_video;
        private string link_card;
        private string description;
        #endregion

        #region Properties
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Description"));
            }
        }
        public string ImageCover
        {
            get { return image_cover; }
            set
            {
                image_cover = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageCover"));
            }
        }
        public string ImagePoster
        {
            get { return image_poster; }
            set
            {
                image_poster = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImagePoster"));
            }
        }
        public string LinkCard
        {
            get { return link_card; }
            set
            {
                link_card = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkCard"));
            }
        }
        public string LinkVideo
        {
            get { return link_video; }
            set
            {
                link_video = value;
                OnPropertyChanged(new PropertyChangedEventArgs("LinkVideo"));
            }
        }
        public string Episode
        {
            get { return episode; }
            set
            {
                episode = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Episode"));
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Title"));
            }
        }
        #endregion
    }
}
