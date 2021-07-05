using HandyControl.Controls;
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
using Tengu.Classes.Extensions;
using Tengu.Classes.Logger;
using Tengu.Classes.Views.Windows;
using Tengu.Classes.WebScrapping;

namespace Tengu.Classes.ViewModels
{
    public class LatestEpisodesViewModel : BindablePropertyBase
    {
        #region Declarations
        private OptimizedObservableCollection<AnimeData> anime_list;
        private List<AnimeData> temp_list;

        private ICommand _CommandNext;
        private ICommand _CommandPrevious;

        private BackgroundWorker refresh_worker;
        private BackgroundWorker btns_worker;

        private int current_page;
        private bool is_loading;
        #endregion

        #region Properties
        public OptimizedObservableCollection<AnimeData> AnimeList
        {
            get { return anime_list; }
            set
            {
                anime_list = value;
                RaisePropertyChanged();
            }
        }
        public ICommand CommandNext
        {
            get
            {
                return _CommandNext;
            }
            set
            {
                _CommandNext = value;
            }
        }
        public ICommand CommandPrevious
        {
            get
            {
                return _CommandPrevious;
            }
            set
            {
                _CommandPrevious = value;
            }
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
        public int CurrentPage
        {
            get { return current_page; }
            set
            {
                current_page = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Constructors
        public LatestEpisodesViewModel()
        {
            AnimeList = new OptimizedObservableCollection<AnimeData>();

            CurrentPage = 1;
            IsLoading = false;

            CommandNext = new SimpleRelayCommand(new Action(NextPage));
            CommandPrevious = new SimpleRelayCommand(new Action(PreviousPage));

            RefreshAnimeList();
        }
        #endregion

        #region Public Methods
        public void RefreshAnimeList()
        {
            if (!IsLoading)
            {
                if (refresh_worker == null)
                {
                    refresh_worker = new BackgroundWorker();
                }

                refresh_worker.DoWork += Refresh_worker_DoWork;
                refresh_worker.RunWorkerCompleted += Refresh_worker_RunWorkerCompleted;

                AnimeList.Clear();

                refresh_worker.RunWorkerAsync();
            }
        }
        #endregion

        #region Private Methods
        private void PreviousPage()
        {
            if (!IsLoading && CurrentPage > 1)
            {
                if (btns_worker == null)
                {
                    btns_worker = new BackgroundWorker();
                }

                btns_worker.DoWork += Btns_worker_DoWork;
                btns_worker.RunWorkerCompleted += Btns_worker_RunWorkerCompleted;

                AnimeList.Clear();

                btns_worker.RunWorkerAsync(false);
            }
        }

        private void NextPage()
        {
            if (!IsLoading)
            {
                if (btns_worker == null)
                {
                    btns_worker = new BackgroundWorker();
                }

                btns_worker.DoWork += Btns_worker_DoWork;
                btns_worker.RunWorkerCompleted += Btns_worker_RunWorkerCompleted;

                AnimeList.Clear();

                btns_worker.RunWorkerAsync(true);
            }
        }
        #endregion

        #region Refresh Worker
        private void Refresh_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                // Refrsh List
                AnimeList.CustomAddRange(temp_list);

                WriteLog("Refresh Completed..");
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);

                ShowErrorMessage("Refresh Latest Episodes Error!", ex.Message);
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
                WriteLog("Refreshing Latest Episodes..");
                IsLoading = true;

                temp_list = Scrapper.Instance.RefreshLatestEpisodes();
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);

                ShowErrorMessage("Refresh Latest Episodes Error!", ex.Message);
                temp_list = new List<AnimeData>();
            }
        }
        #endregion

        #region Buttons Worker
        private void Btns_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if ((bool)e.Result)
                {
                    IsLoading = false;
                    RefreshAnimeList();
                }
                else
                {
                    IsLoading = false;
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Buttons Click Error!", ex.Message);
            }
            finally
            {
                btns_worker.DoWork -= Btns_worker_DoWork;
                btns_worker.RunWorkerCompleted -= Btns_worker_RunWorkerCompleted;
                btns_worker.Dispose();
            }
        }

        private void Btns_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            bool res = false;

            try
            {
                bool arg = (bool)e.Argument;

                IsLoading = true;

                if (arg)
                {
                    if (Scrapper.Instance.NextPage())
                    {
                        CurrentPage++;
                        res = true;
                    }
                }
                else
                {
                    if (Scrapper.Instance.PrevPage())
                    {
                        CurrentPage--;
                        res = true;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage("Buttons Click Error!", ex.Message);
                res = false;
            }

            e.Result = res;
        }
        #endregion

        private void ShowErrorMessage(string title, string message)
        {
            MainWindow.main_window.ShowErrorNotification(title, message);
        }

        #region Debug & Logs
        private void WriteLog(string message)
        {
            Log.Instance.WriteLog("Latest", message);
        }
        private void WriteError(string message)
        {
            Log.Instance.WriteError("Latest", message);
        }
        #endregion
    }
}
