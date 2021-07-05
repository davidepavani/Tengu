using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Classes.DataModels;
using Tengu.Classes.Logger;
using Tengu.Classes.Views.Windows;

namespace Tengu.Classes.ViewModels
{
    public class DownloadViewModel : BindablePropertyBase
    {
        private OptimizedObservableCollection<AnimeData> download_list;
        private OptimizedObservableCollection<HistoryData> download_history;
        private bool is_downloading;

        private BackgroundWorker download_worker;
        private Queue<AnimeData> download_queue;

        private ICommand command_abort_all;
        private ICommand command_clear_history;

        #region Properties
        public ICommand CommandClearHistory
        {
            get { return command_clear_history; }
            set { command_clear_history = value; }
        }
        public ICommand CommandAbortAll
        {
            get { return command_abort_all; }
            set { command_abort_all = value; }
        }
        public OptimizedObservableCollection<AnimeData> DownloadList
        {
            get { return download_list; }
            set
            {
                download_list = value;
                RaisePropertyChanged();
            }
        }
        public bool IsDownloading
        {
            get { return is_downloading; }
            set
            {
                is_downloading = value;
                RaisePropertyChanged();
            }
        }
        public OptimizedObservableCollection<HistoryData> DownloadHistory
        {
            get { return download_history; }
            set
            {
                download_history = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        #region Singleton
        private static DownloadViewModel _instance = null;
        private static readonly object lockObject = new object();

        private DownloadViewModel()
        {
            Initialize();
        }

        public static DownloadViewModel Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new DownloadViewModel();
                    }
                    return _instance;
                }
            }
        }
        #endregion

        public void Initialize()
        {
            IsDownloading = false;
            download_queue = new Queue<AnimeData>();

            CommandClearHistory = new SimpleRelayCommand(ClearHistory);
            CommandAbortAll = new SimpleRelayCommand(AbortAllDownloads);

            DownloadList = new OptimizedObservableCollection<AnimeData>();
            DownloadHistory = new OptimizedObservableCollection<HistoryData>();
        }

        public void EnqueueDownload(AnimeData anime)
        {
            bool to_enqueue = true;

            if (IsDownloading)
            {
                // Check queue
                var matchingvalues = download_queue.Where(val => val.Title == anime.Title &&
                                                                 val.Episode == anime.Episode);

                to_enqueue = matchingvalues.Count() == 0;

                if (to_enqueue)
                {
                    // Check Download List
                    matchingvalues = DownloadList.Where(val => val.Title == anime.Title &&
                                                                val.Episode == anime.Episode);

                    to_enqueue = matchingvalues.Count() == 0;
                }
            }

            if (to_enqueue)
            {
                if(download_worker == null)
                {
                    download_worker = new BackgroundWorker();
                }

                // Enqueue Anime
                download_queue.Enqueue(anime);

                WriteLog("Enqueued Anime: " + anime.FileName);
                DebugAnime(anime);

                if (!download_worker.IsBusy)
                {
                    download_worker.DoWork += Download_worker_DoWork;
                    download_worker.RunWorkerCompleted += Download_worker_RunWorkerCompleted;

                    download_worker.WorkerSupportsCancellation = true;

                    download_worker.RunWorkerAsync();
                }
            }
        }

        public void AbortDownloadWorker()
        {
            try
            {
                if (IsDownloading)
                {
                    if (download_worker != null && download_worker.IsBusy)
                    {
                        download_worker.CancelAsync();
                    }
                }
            }
            catch (Exception)
            {
                // Security..
            }
        }

        public void ClearHistory()
        {
            if (DownloadHistory != null)
            {
                if (DownloadHistory.Count > 0)
                {
                    DownloadHistory.Clear();
                }
            }
        }
        public void AbortAllDownloads()
        {
            try
            {
                if (download_queue != null)
                {
                    if (download_queue.Count > 0)
                    {
                        download_queue.Clear();
                    }
                }

                // Add to History
                foreach (AnimeData anime in DownloadList)
                {
                    // Abort download
                    anime.AbortDownload();

                    AddAnimeToDownloadHistory(new HistoryData()
                    {
                        Title = anime.Title,
                        Episode = anime.Episode,
                        InError = true,
                        ErrorMessage = "Download aborted by the user!"
                    });
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);
                ShowErrorMessage("Abort Error!", ex.Message);
            }
        }

        private void Download_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled) // To test
                {
                    WriteLog("Downloads Aborted by the user!");

                    AbortAllDownloads();
                }
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);

                ShowErrorMessage("Download Error!", ex.Message);
            }
            finally
            {
                download_worker.DoWork -= Download_worker_DoWork;
                download_worker.RunWorkerCompleted -= Download_worker_RunWorkerCompleted;
            }
        }

        private void Download_worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                IsDownloading = true;

                do
                {
                    while(DownloadList.Count > 3)
                    {
                        Thread.Sleep(50);

                        if (download_worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }

                    if (download_worker.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    if (download_queue.Count > 0)
                    {
                        // Dequeue
                        AnimeData data = download_queue.Dequeue();

                        // Add to download list
                        AddAnimeToDownloadList(data);

                        // StartDownload
                        data.StartAsyncDownload();
                    }

                } while (DownloadList.Count > 0 || download_queue.Count > 0);
            }
            catch (Exception ex)
            {
                WriteError(ex.Message);

                ShowErrorMessage("Download Error!", ex.Message);
            }
        }

        public void AddAnimeToDownloadList(AnimeData data)
        {
            DispatcherHelper.RunOnMainThread(() =>
            {
                // Add to download list
                DownloadList.Add(data);
            });
        }
        public void RemoveAnimeToDownloadList(AnimeData data)
        {
            DispatcherHelper.RunOnMainThread(() =>
            {
                // Remove to download list
                DownloadList.Remove(data);

                if(DownloadList.Count == 0)
                {
                    IsDownloading = false;
                }
            });
        }
        public void AddAnimeToDownloadHistory(HistoryData data)
        {
            DispatcherHelper.RunOnMainThread(() =>
            {
                // Add to download list
                DownloadHistory.Add(data);
            });
        }

        private void ShowErrorMessage(string title, string message)
        {
            MainWindow.main_window.ShowErrorNotification(title, message);
        }

        #region Debug & Logs
        private void WriteLog(string message)
        {
            Log.Instance.WriteLog("Dwnld", message);
        }
        private void WriteError(string message)
        {
            Log.Instance.WriteError("Dwnld", message);
        }
        private void DebugAnime(AnimeData anime)
        {
            Log.Instance.DebugAnime(anime);
        }
        #endregion
    }
}
