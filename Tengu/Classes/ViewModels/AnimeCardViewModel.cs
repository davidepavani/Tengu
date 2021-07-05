using HandyControl.Tools;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Classes.DataModels;
using Tengu.Classes.Views.Windows;
using Tengu.Classes.WebScrapping;
using Tengu.Classes.Extensions;
using Tengu.Classes.Logger;

namespace Tengu.Classes.ViewModels
{
    public class AnimeCardViewModel : BindablePropertyBase
    {
        private string anime_card_url;
        private bool is_loading;
        private BackgroundWorker refresh_worker;

        private ICommand command_go_back;

        private CardData anime_card;
        private CardData temp_card;

        #region Properties
        public CardData AnimeCard
        {
            get { return anime_card; }
            set
            {
                anime_card = value;
                RaisePropertyChanged();
            }
        }
        public ICommand CommandGoBack
        {
            get { return command_go_back; }
            set { command_go_back = value; }
        }
        public bool IsLoading
        {
            get { return is_loading; }
            set
            {
                is_loading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public AnimeCardViewModel(string url)
        {
            anime_card_url = url;

            anime_card = new CardData();
            temp_card = new CardData();

            CommandGoBack = new SimpleRelayCommand(GoBack);

            RefreshData();
        }

        private void GoBack()
        {
            MainWindow.main_window.NavigateBack();
        }

        public void RefreshData()
        {
            if (!IsLoading)
            {
                if (refresh_worker == null)
                {
                    refresh_worker = new BackgroundWorker();
                }

                WriteLog("Refreshing card..");

                refresh_worker.DoWork += Refresh_worker_DoWork; ;
                refresh_worker.RunWorkerCompleted += Refresh_worker_RunWorkerCompleted;

                refresh_worker.RunWorkerAsync();
            }
        }

        #region BackgroundWorker
        private void Refresh_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                WriteLog("Card Refresh Completed");

                AnimeCard.Title = temp_card.Title;
                AnimeCard.Plot = temp_card.Plot;
                AnimeCard.ImageLink = temp_card.ImageLink;

                AnimeCard.Tags.CustomAddRange(temp_card.Tags);
                AnimeCard.UsefulLinks.CustomAddRange(temp_card.UsefulLinks);
                AnimeCard.Related.CustomAddRange(temp_card.Related);
                AnimeCard.Attributes.CustomAddRange(temp_card.Attributes);
                AnimeCard.Episodes.CustomAddRange(temp_card.Episodes);

                DebugCard(temp_card);
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);

                ShowErrorMessage("Anime Card Error!", ex.Message);
            }
            finally
            {
                IsLoading = false;

                refresh_worker.DoWork -= Refresh_worker_DoWork;
                refresh_worker.RunWorkerCompleted -= Refresh_worker_RunWorkerCompleted;
                refresh_worker.Dispose();
            }
        }

        private void Refresh_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IsLoading = true;

                // Scrapper: Refresh Anime Card
                temp_card = Scrapper.Instance.RefreshAnimeCard(anime_card_url);
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);

                ShowErrorMessage("Anime Card Error!", ex.Message);
            }
        }
        #endregion

        private void ShowErrorMessage(string title, string message)
        {
            MainWindow.main_window.ShowErrorNotification(title, message);
        }

        #region Debug & Logs
        private void WriteLog(string message)
        {
            Log.Instance.WriteLog("Card", message);
        }
        private void WriteError(string message)
        {
            Log.Instance.WriteError("Card", message);
        }
        private void DebugCard(CardData card)
        {
            Log.Instance.DebugCard(card);
        }
        #endregion
    }
}
