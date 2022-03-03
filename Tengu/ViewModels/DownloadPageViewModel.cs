using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tengu.ViewModels
{
    public class DownloadPageViewModel : ReactiveObject
    {
        public List<string> AnimeUnityQueue { get; set; }

        public DownloadPageViewModel()
        {
            AnimeUnityQueue = new();

            AnimeUnityQueue.Add("123");
            AnimeUnityQueue.Add("123");
            AnimeUnityQueue.Add("123");
        }
    }
}
