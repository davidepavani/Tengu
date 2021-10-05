using HandyControl.Controls;
using HandyControl.Tools;
using HandyControl.Tools.Command;
using NLog;
using Notification.Wpf;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Models;
using static Tengu.Utilities.PrismEvents;
using Logger = NLog.Logger;

namespace Tengu.ViewModels.DownloadControlsViewModels
{
    public class QueueUserControlViewModel : BindablePropertyBase, IDisposable
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private readonly IRegionManager _regionManager;
        private IEventAggregator _eventAggregator;

        private OptimizedObservableCollection<AnimeData> download_list;

        private NotificationManager notificationManager;
        private bool is_downloading;

        private BackgroundWorker download_worker;
        private Queue<AnimeData> download_queue;

        public ICommand CommandAbortAll { get; private set; }
        public ICommand CommandNavigateToHistory { get; private set; }
        public ICommand CommandPauseDownload { get; private set; }
        public ICommand CommandResumeDownload { get; private set; }
        public ICommand CommandAbortDownload { get; private set; }
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

        public QueueUserControlViewModel() { }
        public QueueUserControlViewModel(IRegionManager regionManager, IEventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            download_queue = new Queue<AnimeData>();
            DownloadList = new OptimizedObservableCollection<AnimeData>();
            notificationManager = new NotificationManager();

            _eventAggregator.GetEvent<AddAnimeToDownloadListEvent>().Subscribe(EnqueueDownload);
            _eventAggregator.GetEvent<RemoveAnimeToDownloadListEvent>().Subscribe(RemoveAnime);
            _eventAggregator.GetEvent<AbortAllDownloadsEvent>().Subscribe(AbortDownloadWorker);

            CommandPauseDownload = new RelayCommand<AnimeData>(PauseAnime);
            CommandResumeDownload = new RelayCommand<AnimeData>(ResumeAnime);
            CommandAbortDownload = new RelayCommand<AnimeData>(AbortAnime);

            CommandAbortAll = new SimpleRelayCommand(AbortAllDownloads);
            CommandNavigateToHistory = new SimpleRelayCommand(Navigate);
        }

        public void Navigate()
        {
            _regionManager.RequestNavigate("DwnldFrame", "HistoryUserControl");
        }

        private void AddAnime(AnimeData anime)
        {
            DownloadList.Add(anime);
        }
        private void RemoveAnime(AnimeData anime)
        {
            DispatcherHelper.RunOnMainThread(() =>
            {
                DownloadList.Remove(anime);

                if (DownloadList.Count == 0)
                {
                    IsDownloading = false;
                    _eventAggregator.GetEvent<DownloadEvent>().Publish(false);
                }
            });
        }

        private void AbortAnime(AnimeData anime)
        {
            if (IsDownloading)
            {
                try
                {
                    AnimeData matched_anime = DownloadList.Single(val => val.Title == anime.Title && val.Episode == anime.Episode);

                    if (matched_anime != null)
                    {
                        matched_anime.AbortDownload();
                    }
                }
                catch(Exception ex)
                {
                    log.Error("Abort anime >> " + anime.Title + " - Episode " + anime.Episode + " >> " + ex.Message);
                }
            }
        }
        private void PauseAnime(AnimeData anime)
        {
            if (IsDownloading)
            {
                try
                {
                    AnimeData matched_anime = DownloadList.Single(val => val.Title == anime.Title && val.Episode == anime.Episode);

                    if (matched_anime != null && !matched_anime.IsPaused)
                    {
                        matched_anime.PauseDownload();
                    }
                }
                catch(Exception ex)
                {
                    log.Error("Pause anime >> " + anime.Title + " - Episode " + anime.Episode + " >> " + ex.Message);
                }
            }
        }
        private void ResumeAnime(AnimeData anime)
        {
            if (IsDownloading)
            {
                try
                {
                    AnimeData matched_anime = DownloadList.Single(val => val.Title == anime.Title && val.Episode == anime.Episode);

                    if (matched_anime != null && matched_anime.IsPaused)
                    {
                        matched_anime.ResumeDownload();
                    }
                }
                catch(Exception ex)
                {
                    log.Error("Resume anime >> " + anime.Title + " - Episode " + anime.Episode + " >> " + ex.Message);
                }
            }
        }

        public void EnqueueDownload(AnimeData anime)
        {
            bool to_enqueue = true;

            if (IsDownloading)
            {
                // Check queue
                var matchingvalues = download_queue.Where(val => val.Title == anime.Title &&
                                                                 val.Episode == anime.Episode);

                to_enqueue = !matchingvalues.Any();

                if (to_enqueue)
                {
                    // Check Download List
                    matchingvalues = DownloadList.Where(val => val.Title == anime.Title &&
                                                                val.Episode == anime.Episode);

                    to_enqueue = !matchingvalues.Any();
                }
            }

            if (to_enqueue)
            {
                if (download_worker == null)
                {
                    download_worker = new BackgroundWorker();
                }

                // Enqueue Anime
                download_queue.Enqueue(anime);

                log.Info("Enqueued Anime: " + anime.FileName);

                if (!download_worker.IsBusy)
                {
                    download_worker.DoWork += Download_worker_DoWork;
                    download_worker.RunWorkerCompleted += Download_worker_RunWorkerCompleted;

                    download_worker.WorkerSupportsCancellation = true;

                    download_worker.RunWorkerAsync();
                }
            }
        }

        private void Download_worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled) // To test
                {
                    log.Info("Downloads Aborted by the user!");

                    AbortAllDownloads();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                ShowErrorNotification("Download Error!", ex.Message);
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

                _eventAggregator.GetEvent<DownloadEvent>().Publish(true);

                do
                {
                    while (DownloadList.Count > 3)
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

                        DispatcherHelper.RunOnMainThread(() =>
                        {
                            // Add to download list
                            AddAnime(data);
                        });


                        // StartDownload
                        data.StartAsyncDownload();
                    }

                } while (DownloadList.Count > 0 || download_queue.Count > 0);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                ShowErrorNotification("Download Error!", ex.Message);
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
                        download_worker.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                // Security..
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

                List<AnimeData> list = new List<AnimeData>();
                list.AddRange(DownloadList);

                // Add to History
                foreach (AnimeData anime in list)
                {
                    // Abort download
                    anime.AbortDownload();

                    HistoryData h = new HistoryData()
                    {
                        Title = anime.Title,
                        Episode = anime.Episode,
                        InError = true,
                        ErrorMessage = "Download aborted by the user!"
                    };

                    //_eventAggregator.GetEvent<AddAnimeToDownloadHistoryEvent>().Publish(h);
                    RemoveAnime(anime);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ShowErrorNotification("Abort Error!", ex.Message);
            }
        }

        public void ShowErrorNotification(string title, string message)
        {
            DispatcherHelper.RunOnMainThread(() =>
            {
                notificationManager.Show(new NotificationContent
                {
                    Title = title,
                    Message = message,
                    Type = NotificationType.Error
                });
            });
        }

        public void Dispose()
        {
            AbortDownloadWorker();
        }
    }
}
