using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Shared.Utilities;
using System.ComponentModel;

namespace Tengu.Models
{
    public partial class Card_Data : NotifyPropertyChanged
    {
        private string title;
        private string image_link;
        private string plot;

        private ObservableCollection<string> tags;
        private ObservableCollection<UsefulLinkData> useful_links;
        private ObservableCollection<RelatedData> related;
        private ObservableCollection<DictionaryData> attributes;
        private ObservableCollection<Anime> episodes;

        #region Properties
        public ObservableCollection<UsefulLinkData> UsefulLinks
        {
            get { return useful_links; }
            set
            {
                useful_links = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UsefulLinks"));
            }
        }
        public ObservableCollection<RelatedData> Related
        {
            get { return related; }
            set
            {
                related = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Related"));
            }
        }
        public ObservableCollection<Anime> Episodes
        {
            get { return episodes; }
            set
            {
                episodes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Episodes"));
            }
        }
        public ObservableCollection<DictionaryData> Attributes
        {
            get { return attributes; }
            set
            {
                attributes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attributes"));
            }
        }
        public ObservableCollection<string> Tags
        {
            get { return tags; }
            set
            {
                tags = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tags"));
            }
        }
        public string Plot
        {
            get { return plot; }
            set
            {
                plot = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Plot"));
            }
        }
        public string ImageLink
        {
            get { return image_link; }
            set
            {
                image_link = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ImageLink"));
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

        #region Constructors
        public Card_Data()
        {
            title = "Unknown";
            image_link = string.Empty;
            plot = string.Empty;

            Tags = new ObservableCollection<string>();
            UsefulLinks = new ObservableCollection<UsefulLinkData>();
            Attributes = new ObservableCollection<DictionaryData>();
            Episodes = new ObservableCollection<Anime>();
            Related = new ObservableCollection<RelatedData>();
        }
        #endregion
    }
}
