using HandyControl.Tools;
using System;
using Tengu.Classes.Extensions;
using System.IO;
using Tengu.Classes.Utilities;
using System.Threading;
using Tengu.Classes.Logger;
using Tengu.Classes.ViewModels;
using Tengu.Classes.Views.Windows;
using System.Diagnostics;
using System.Windows.Input;
using HandyControl.Tools.Command;
using System.Management;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Tengu.Classes.DataModels
{
    public class AnimeData : BindablePropertyBase
    {
        #region Declarations
        private string title;
        private string episode;
        private string image_poster;
        private string image_cover;
        private string link_video;
        private string link_card;
        private string description;

        private double download_percentage;
        private bool is_downloading;
        private bool is_paused;

        private ICommand command_enqueue;
        private ICommand command_show_card;

        private ICommand command_pause;
        private ICommand command_resume;
        private ICommand command_abort;

        private Process ytdl;
        #endregion

        #region Properties
        public ICommand CommandAbort
        {
            get { return command_abort; }
            set { command_abort = value; }
        }
        public ICommand CommandResume
        {
            get { return command_resume; }
            set { command_resume = value; }
        }
        public ICommand CommandPause
        {
            get { return command_pause; }
            set { command_pause = value; }
        }
        public ICommand CommandEnqueue
        {
            get { return command_enqueue; }
            set { command_enqueue = value; }
        }
        public ICommand CommandShowCard
        {
            get { return command_show_card; }
            set { command_show_card = value; }
        }
        public string Description
        {
            get { return description; }
            set
            {
                description = value;
                RaisePropertyChanged();
            }
        }
        private string FullPath
        {
            get
            {
                return Path.Combine(string.IsNullOrEmpty(ProgramInfo.Instance.DownloadFolder) ?
                                    KnownFolders.GetPath(KnownFolder.Downloads) :
                                    ProgramInfo.Instance.DownloadFolder, 
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
        public string ImageCover
        {
            get { return image_cover; }
            set
            {
                image_cover = value;
                RaisePropertyChanged();
            }
        }
        public string ImagePoster
        {
            get { return image_poster; }
            set
            {
                image_poster = value;
                RaisePropertyChanged();
            }
        }
        public bool IsPaused
        {
            get { return is_paused; }
            set
            {
                is_paused = value;
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
        public double DownloadPercentage
        {
            get { return download_percentage; }
            set
            {
                download_percentage = value;
                RaisePropertyChanged();
            }
        }
        public string LinkCard
        {
            get { return link_card; }
            set
            {
                link_card = value;
                RaisePropertyChanged();
            }
        }
        public string LinkVideo
        {
            get { return link_video; }
            set
            {
                link_video = value;
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
        public AnimeData()
        {
            CommandEnqueue = new SimpleRelayCommand(Enqueue);
            CommandShowCard = new SimpleRelayCommand(ShowCard);

            CommandPause = new SimpleRelayCommand(PauseDownload);
            CommandResume = new SimpleRelayCommand(ResumeDownload);
            CommandAbort = new SimpleRelayCommand(AbortDownload);
        }
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

                ytdl.Start();
                ytdl.BeginOutputReadLine();
            }
            catch(Exception ex)
            {
                ShowErrorMessage("Download Error!", ex.Message);
                PerformEnd(ex.Message);
            }
        }

        private void ytdl_ErrorReceived(object sender, DataReceivedEventArgs e)
        {
            WriteDebug("ErrorReceived >> " + e.Data);

            if (!IsPaused)
            {
                PerformEnd(e.Data);
            }
        }

        private void ytdl_OutputReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && !string.IsNullOrEmpty(e.Data))
            {
                WriteDebug(e.Data);

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

                    PerformEnd("Download Aborted by the User!");

                    if (!ProgramInfo.Instance.KeepPartialDownloads)
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

                    IsPaused = false;
                    IsDownloading = false;
                }
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

            ytdl.StartInfo.Arguments = "--all-subs -f mp4 -o \"" + FullPath + "\" \"" + this.GetRealVideoUrl() + "\""; // a live stream
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
            WriteError(error_message);

            DownloadViewModel.Instance.RemoveAnimeToDownloadList(this);

            HistoryData history = new HistoryData();

            history.Title = this.Title;
            history.Episode = this.Episode;
            history.ErrorMessage = error_message;
            history.InError = !string.IsNullOrEmpty(error_message);

            DownloadViewModel.Instance.AddAnimeToDownloadHistory(history);

            IsDownloading = false;
        }

        #endregion

        #region Debug & Logs
        private void WriteLog(string message)
        {
            Log.Instance.WriteLog("Anime", message);
        }
        private void WriteError(string message)
        {
            Log.Instance.WriteError("Anime", message);
        }
        private void WriteDebug(string message)
        {
            Log.Instance.DebugDownload(message);
        }
        #endregion

        private void Enqueue()
        {
            DownloadViewModel.Instance.EnqueueDownload(this);
        }
        private void ShowCard()
        {
            this.GetAnimeCardFromVideoUrl();
            MainWindow.main_window.NavigateToAnimeCardPage(this.LinkCard);
        }
        private void ShowErrorMessage(string title, string message)
        {
            MainWindow.main_window.ShowErrorNotification(title, message);
        }
    }
}
