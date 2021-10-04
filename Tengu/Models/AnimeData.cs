using System;
using Tengu.WebScrapper;
using System.IO;
using Tengu.Utilities;
using System.Threading;
using Tengu.ViewModels;
using Tengu.Views.Windows;
using System.Diagnostics;
using System.Windows.Input;
using HandyControl.Tools.Command;
using System.Globalization;
using System.Text.RegularExpressions;
using System.ComponentModel;
using Tengu.WebScrapper.Extensions;
using System.Management;
using Tengu.WebScrapper.Helpers;
using NLog;
using Prism.Events;
using static Tengu.Utilities.PrismEvents;
using Notification.Wpf;

namespace Tengu.Models
{
    public class AnimeData : Anime
    {
        #region Declarations
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private NotificationManager notificationManager;
        private IEventAggregator _eventAggregator;

        private double download_percentage;
        private bool is_downloading;
        private bool is_paused;

        private Process ytdl;
        #endregion

        #region Properties
        private string FullPath
        {
            get
            {
                return Path.Combine(string.IsNullOrEmpty(ProgramInfos.Instance.DownloadFolder) ?
                                    KnownFolders.GetPath(KnownFolder.Downloads) :
                                    ProgramInfos.Instance.DownloadFolder, 
                                    FileName);
            }
        }
        public string FileName
        {
            get 
            {
                return string.Format("{0} - {1}.{2}", Title.MakeValidFileName(), Episode, "mp4");
            }
        }
        public bool IsPaused
        {
            get { return is_paused; }
            set
            {
                is_paused = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsPaused"));
            }
        }
        public bool IsDownloading
        {
            get { return is_downloading; }
            set
            {
                is_downloading = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsDownloading"));
            }
        }
        public double DownloadPercentage
        {
            get { return download_percentage; }
            set
            {
                download_percentage = value;
                OnPropertyChanged(new PropertyChangedEventArgs("DownloadPercentage"));
            }
        }
        #endregion

        #region Constructors
        public AnimeData(IEventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            notificationManager = new NotificationManager();
        }
        public AnimeData() { }
        #endregion

        #region Public Methods

        public void StartAsyncDownload()
        {
            try
            {
                // Initialize
                IsPaused = false;
                DownloadPercentage = 0;

                InitializeProcess();

                IsDownloading = true;

                bool test = ytdl.Start();
                ytdl.BeginOutputReadLine();
            }
            catch(Exception ex)
            {
                ShowErrorNotification("Download Error!", ex.Message);
                PerformEnd(ex.Message);
            }
        }

        private void ytdl_ErrorReceived(object sender, DataReceivedEventArgs e)
        {
            log.Debug("ErrorReceived >> " + e.Data);

            if (!IsPaused)
            {
                PerformEnd(e.Data);
            }
        }

        private void ytdl_OutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && !string.IsNullOrEmpty(e.Data))
            {
                log.Debug(e.Data);

                Regex r = new Regex(@"(\d+(\.\d+)?%)");

                if (r.IsMatch(e.Data))
                {
                    double res = Regex.Replace(r.Match(e.Data).Value, "[%]", "").ConvertToDouble();

                    if (res != 0)
                    {
                        DownloadPercentage = Convert.ToDouble(res);

                        if (DownloadPercentage == 100)
                        {
                            PerformEnd(string.Empty);
                        }
                    }
                }
            }
        }

        public void AbortDownload()
        {
            if (IsDownloading)
            {
                if (ytdl != null && !ytdl.HasExited)
                {
                    KillProcessAndChildren(ytdl.Id);

                    if (!ProgramInfos.Instance.KeepPartialDownloads)
                    {
                        DirectoryInfo d = new DirectoryInfo(Path.GetDirectoryName(FullPath)); // folder info
                        foreach (FileInfo file in d.GetFiles($"*{FileName}*"))
                        {
                            while (!IsFileReady(file.FullName))
                            {
                                Thread.Sleep(50);
                            }

                            // delete dwnld files
                            file.Delete();
                        }
                    }
                }

                PerformEnd("Download Aborted by the User!");

                IsPaused = false;
                IsDownloading = false;
            }
        }

        private void KillProcessAndChildren(int pid)
        {
            // Cannot close 'system idle process'.
            if (pid == 0)
            {
                return;
            }
            ManagementObjectSearcher searcher = new ManagementObjectSearcher
                    ("Select * From Win32_Process Where ParentProcessID=" + pid);
            ManagementObjectCollection moc = searcher.Get();
            foreach (ManagementObject mo in moc)
            {
                KillProcessAndChildren(Convert.ToInt32(mo["ProcessID"]));
            }
            try
            {
                Process proc = Process.GetProcessById(pid);
                proc.Kill();
            }
            catch (ArgumentException)
            {
                // Process already exited.
            }
        }

        private void InitializeProcess()
        {
            ytdl = new Process();

            ytdl.StartInfo.FileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                   "youtube-dl",
                                                   "youtube-dl.exe");

            ytdl.StartInfo.ArgumentList.Add("--all-subs"); // Download all the available subtitles of the video
            ytdl.StartInfo.ArgumentList.Add("--no-check-certificate"); // Suppress HTTPS certificate validation
            ytdl.StartInfo.ArgumentList.Add("-f mp4"); // Video format code

            ytdl.StartInfo.ArgumentList.Add("-o"); // Output filename template
            ytdl.StartInfo.ArgumentList.Add(FullPath); // Output filename

            ytdl.StartInfo.ArgumentList.Add(AnimeDataHelper.GetRealVideoUrl(LinkVideo)); // Video url

            ytdl.StartInfo.UseShellExecute = false;
            ytdl.StartInfo.RedirectStandardOutput = true;
            ytdl.StartInfo.RedirectStandardInput = true;
            ytdl.StartInfo.CreateNoWindow = true;

            ytdl.OutputDataReceived += ytdl_OutputReceived;
            ytdl.ErrorDataReceived += ytdl_ErrorReceived;
        }

        private bool IsFileReady(string filename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None))
                    return inputStream.Length > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public void PauseDownload()
        {
            if (IsDownloading)
            {
                KillProcessAndChildren(ytdl.Id);
                
                IsPaused = true;
            }
        }

        public void ResumeDownload()
        {
            if(IsDownloading && IsPaused)
            {
                InitializeProcess();

                ytdl.Start();
                ytdl.BeginOutputReadLine();

                IsPaused = false;
            }
        }

        private void PerformEnd(string error_message)
        {
            log.Error(error_message);

            _eventAggregator.GetEvent<RemoveAnimeToDownloadListEvent>().Publish(this);
            // DownloadUserControlViewModel.Instance.RemoveAnimeToDownloadList(this);

            HistoryData history = new HistoryData();

            history.Title = this.Title;
            history.Episode = this.Episode;
            history.ErrorMessage = error_message;
            history.InError = !string.IsNullOrEmpty(error_message);

            _eventAggregator.GetEvent<AddAnimeToDownloadHistoryEvent>().Publish(history);
            //DownloadUserControlViewModel.Instance.AddAnimeToDownloadHistory(history);

            ytdl.OutputDataReceived -= ytdl_OutputReceived;
            ytdl.ErrorDataReceived -= ytdl_ErrorReceived;

            IsDownloading = false;
        }

        public void ShowErrorNotification(string title, string message)
        {
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Error
            });
        }
        #endregion
    }
}
