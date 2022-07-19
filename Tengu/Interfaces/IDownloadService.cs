using Avalonia.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Models;
using Tengu.Models;

namespace Tengu.Interfaces
{
    public interface IDownloadService
    {
        AvaloniaList<DownloadModel> CurrentDownloads { get; } // Current animes in Download: 1 For Unity and 1 For Saturn
        AvaloniaList<DownloadModel> AnimeQueue { get; }
        AvaloniaList<HistoryModel> HistoryList { get; }
        int DownloadCount { get; }
        void EnqueueAnime(EpisodeModel episode);
        void CancelDownload(DownloadModel download);
    }
}
