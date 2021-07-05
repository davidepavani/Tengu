using HandyControl.Tools;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Classes.Views.Windows;

namespace Tengu.Classes.DataModels
{
    public class RelatedData : BindablePropertyBase
    {
        private string title;
        private string link;
        private string image;

        private ICommand command_show_card;

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
        public string Image
        {
            get { return image; }
            set
            {
                image = value;
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
        public ICommand CommandShowCard
        {
            get { return command_show_card; }
            set
            {
                command_show_card = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public RelatedData()
        {
            CommandShowCard = new SimpleRelayCommand(ShowCard);
        }

        public void ShowCard()
        {
            MainWindow.main_window.NavigateToAnimeCardPage(this.Link);
        }
    }
}
