using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Classes.Extensions;

namespace Tengu.Classes.DataModels
{
    public class CardData : BindablePropertyBase
    {
        private string title;
        private string image_link;
        private string plot;

        private OptimizedObservableCollection<string> tags;
        private OptimizedObservableCollection<UsefulLinkData> useful_links;
        private OptimizedObservableCollection<RelatedData> related;
        private OptimizedObservableCollection<DictionaryData> attributes;
        private OptimizedObservableCollection<AnimeData> episodes;

        #region Properties
        public OptimizedObservableCollection<UsefulLinkData> UsefulLinks
        {
            get { return useful_links; }
            set
            {
                useful_links = value;
                RaisePropertyChanged();
            }
        }
        public OptimizedObservableCollection<RelatedData> Related
        {
            get { return related; }
            set
            {
                related = value;
                RaisePropertyChanged();
            }
        }
        public OptimizedObservableCollection<AnimeData> Episodes
        {
            get { return episodes; }
            set
            {
                episodes = value;
                RaisePropertyChanged();
            }
        }
        public OptimizedObservableCollection<DictionaryData> Attributes
        {
            get { return attributes; }
            set
            {
                attributes = value;
                RaisePropertyChanged();
            }
        }
        public OptimizedObservableCollection<string> Tags
        {
            get { return tags; }
            set
            {
                tags = value;
                RaisePropertyChanged();
            }
        }
        public string Plot
        {
            get { return plot; }
            set
            {
                plot = value;
                RaisePropertyChanged();
            }
        }
        public string ImageLink
        {
            get { return image_link; }
            set
            {
                image_link = value;
                RaisePropertyChanged();
            }
        }
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public CardData()
        {
            title = "Unknown";
            image_link = string.Empty;
            plot = string.Empty;

            Tags = new OptimizedObservableCollection<string>();
            UsefulLinks = new OptimizedObservableCollection<UsefulLinkData>();
            Attributes = new OptimizedObservableCollection<DictionaryData>();
            Episodes = new OptimizedObservableCollection<AnimeData>();
            Related = new OptimizedObservableCollection<RelatedData>();
        }
        #endregion
    }
}
