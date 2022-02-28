using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Enums;
using Tengu.Extensions;
using Tengu.Utilities;

namespace Tengu.ViewModels
{
    public class LatestEpisodesPageViewModel : ReactiveObject
    {
        private CustomObservableCollection<string> animeList = new(); // MODEL
        private TenguHost selectedHost = TenguHost.AnimeSaturn;
        private bool loadingAnimes = false;

        #region Properties
        public List<TenguHost> HostList { get; private set; }
        public bool LoadingAnimes
        {
            get => loadingAnimes;
            set => this.RaiseAndSetIfChanged(ref loadingAnimes, value);
        }
        public CustomObservableCollection<string> AnimeList
        {
            get => animeList;
            set => this.RaiseAndSetIfChanged(ref animeList, value);
        }
        public TenguHost SelectedHost
        {
            get => selectedHost;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedHost, value);

                // todo update
            }
        }
        #endregion

        public LatestEpisodesPageViewModel()
        {
            HostList = EnumExtension.ToList<TenguHost>();

        }

        private void ReloadAnimes()
        {
            Task.Run(() =>
            {
                LoadingAnimes = true;

                AnimeList.Clear();

            }).GetAwaiter().OnCompleted(() =>
            {
                // MessageBox.Show("the task completed in the main thread", "");

                LoadingAnimes = false;
            });
        }

    }
}
