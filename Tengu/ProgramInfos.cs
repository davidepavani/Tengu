using HandyControl.Tools;
using Newtonsoft.Json;
using NLog;
using Notification.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.WebScrapper.Extensions;
using Logger = NLog.Logger;

namespace Tengu
{
    public class ProgramInfos : BindablePropertyBase
    {
        public const string APPLICATION_NAME = "Tengu";
        public const string DISPLAY_NAME = "Tengu.";

        public const string COMPANY_NAME = "Dugong's Software";
        public const string COPYRIGHT = "Copyright © Dugong 2021";

        public const string APPLICATION_VERSION = "1.2.*";
        public const string ASSEMBLY_FILE_VERSION = "1.0.0";

        // ---------------------------------------------------------------------------------

        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private NotificationManager notificationManager;

        public const string FILE_NAME = APPLICATION_NAME + ".json";
        public const int FILE_VERSION = 1;

        #region Declarations

        private int file_version;
        private string download_folder = string.Empty;
        private bool notify_on_download_completed = true;
        private bool keep_partial_downloads = false;
        private bool enable_log = true;
        private bool log_anime_data = false;
        private bool log_downloads = false;

        #endregion

        #region Singleton
        private static ProgramInfos _instance = null;
        private static readonly object lockObject = new object();

        private ProgramInfos()
        {
            // ..
            notificationManager = new NotificationManager();
        }

        public static ProgramInfos Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new ProgramInfos();
                    }
                    return _instance;
                }
            }
        }
        #endregion

        #region Properties
        public int FileVersion
        {
            get { return file_version; }
            set
            {
                file_version = value;
                RaisePropertyChanged();
            }
        }
        public string DownloadFolder
        {
            get { return download_folder; }
            set
            {
                download_folder = value;
                RaisePropertyChanged();
            }
        }
        public bool NotifyOnDownloadCompleted
        {
            get { return notify_on_download_completed; }
            set
            {
                notify_on_download_completed = value;
                RaisePropertyChanged();
            }
        }
        public bool KeepPartialDownloads
        {
            get { return keep_partial_downloads; }
            set
            {
                keep_partial_downloads = value;
                RaisePropertyChanged();
            }
        }
        public bool EnableLog
        {
            get { return enable_log; }
            set
            {
                enable_log = value;
                RaisePropertyChanged();
            }
        }
        public bool LogAnimeData
        {
            get { return log_anime_data; }
            set
            {
                log_anime_data = value;
                RaisePropertyChanged();
            }
        }
        public bool LogDownloads
        {
            get { return log_downloads; }
            set
            {
                log_downloads = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        public void LoadConfiguration()
        {
            try
            {
                if (!File.Exists(FILE_NAME))
                {
                    SaveConfiguration();
                }

                string json = File.ReadAllText(FILE_NAME);
                _instance = JsonConvert.DeserializeObject<ProgramInfos>(json, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                if (_instance.file_version < FILE_VERSION)
                {
                    _instance.file_version = FILE_VERSION;
                    SaveConfiguration(); // Update File
                }

                log.Info("Configuration loaded: " + json.NormalizeString());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ShowErrorNotification("Configuration Load Error!", ex.Message);
            }
        }

        public void SaveConfiguration()
        {
            try
            {
                string json = JsonConvert.SerializeObject(Instance, Formatting.Indented, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                File.WriteAllText(FILE_NAME, json);

                log.Info("Configuration saved: " + json.NormalizeString());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                ShowErrorNotification("Configuration Save Error!", ex.Message);
            }
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
    }
}
