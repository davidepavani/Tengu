using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Tengu.Classes.DataModels;
using Tengu.Classes.Extensions;
using Tengu.Classes.Enums;

namespace Tengu.Classes.Logger
{
    public class Log
    {
        private const int _MAX_SIZE = (int)2e+8; // 200 MB
        private const string _MUTEX_NAME = "TenguLogMutex";

        private TextWriterTraceListener log_listener;
        private Mutex objMutex;

        #region Properties
        private string CurrentDate
        {
            get
            {
                return DateTime.Now.ToString("ddMMMMyyyy");
            }
        }
        private string CurrentTime
        {
            get
            {
                return DateTime.Now.ToString("HH:mm:ss fff");
            }
        }
        private string FileName
        {
            get 
            {
                return string.Format("{0}_{1}.log", ProgramInfo._APP_NAME, CurrentDate);
            }
        }
        private string FullName
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs", FileName);
            }
        }
        public string LogDirectory
        {
            get { return Path.GetDirectoryName(FullName); }
        }
        #endregion

        #region Singleton
        private static Log _instance = null;
        private static readonly object lockObject = new object();

        private Log()
        {
            Initialize();
        }

        public static Log Instance
        {
            get
            {
                lock (lockObject)
                {
                    if (_instance == null)
                    {
                        _instance = new Log();
                    }
                    return _instance;
                }
            }
        }
        #endregion

        private void Initialize()
        {
            // Create Named Mutex
            objMutex = new Mutex(false, _MUTEX_NAME);

            if (!Directory.Exists(LogDirectory))
            {
                // Create log Directory if not exists
                Directory.CreateDirectory(LogDirectory);
            }

            // Delete files older than 7 days
            foreach (string file in Directory.GetFiles(LogDirectory))
            {
                FileInfo info = new FileInfo(file);

                if (info.LastAccessTime < DateTime.Now.AddDays(-7) || 
                    info.Length >= _MAX_SIZE)
                {
                    info.Delete();
                }
            }

            // Initialize Listeners
            log_listener = new TextWriterTraceListener(FullName);
        }

        public void WriteLog(string TAG, string message)
        {
            lock (objMutex)
            {
                if (ProgramInfo.Instance.EnableLog)
                {
                    log_listener.WriteLine(string.Format("{0} | [{1}] {2}", CurrentTime, TAG, message));
                    log_listener.Flush();
                }
            }
        }

        public void WriteError(string TAG, string message)
        {
            lock (objMutex)
            {
                if (ProgramInfo.Instance.EnableLog)
                {
                    log_listener.WriteLine(string.Format("{0} | [{1}] Error: {2}", CurrentTime, TAG, message));
                    log_listener.Flush();
                }
            }
        }

        #region Write Debug
        public void DebugCard(CardData data)
        {
            lock (objMutex)
            {
                if (ProgramInfo.Instance.EnableLog &&
                    ProgramInfo.Instance.LogAnimeData)
                {
                    log_listener.WriteLine(string.Format("{0} | [Card DATA] Dbg: {1}", CurrentTime, data.ConvertToJson()));
                    log_listener.Flush();
                }
            }
        }
        public void DebugAnime(AnimeData data)
        {
            lock (objMutex)
            {
                if (ProgramInfo.Instance.EnableLog &&
                    ProgramInfo.Instance.LogAnimeData)
                {
                    log_listener.WriteLine(string.Format("{0} | [Anime DATA] Dbg: {1}", CurrentTime, data.ConvertToJson()));
                    log_listener.Flush();
                }
            }
        }
        public void DebugDownload(string message)
        {
            lock (objMutex)
            {
                if (ProgramInfo.Instance.EnableLog &&
                    ProgramInfo.Instance.LogDownloads)
                {
                    log_listener.WriteLine(string.Format("{0} | [youtube-dl] dbg: {1}", CurrentTime, message));
                    log_listener.Flush();
                }
            }
        }
        #endregion

        public void CloseLog()
        {
            try
            {
                // Dispose Listener
                if(log_listener != null)
                {
                    log_listener.Close();
                    log_listener.Dispose();
                }

                // Release Mutex
                if (objMutex != null)
                {
                    objMutex.ReleaseMutex();
                    objMutex.Close();
                }
            }
            catch (Exception)
            {
                // Security ..
            }
        }
    }
}
