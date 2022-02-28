using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
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
        private int currentPage = 0;

        private bool canPrev = false;

        #region Properties
        public ICommand NextPageCommand { get; private set; }
        public ICommand PrevPageCommand { get; private set; }
        public List<TenguHost> HostList { get; private set; }
        public bool CanPrev
        {
            get => canPrev;
            set => this.RaiseAndSetIfChanged(ref canPrev, value);
        }
        public bool LoadingAnimes
        {
            get => loadingAnimes;
            set => this.RaiseAndSetIfChanged(ref loadingAnimes, value);
        }
        public int CurrentPage
        {
            get => currentPage;
            set
            {
                this.RaiseAndSetIfChanged(ref currentPage, value);
                CanPrev = !value.Equals(0);
            }
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

            AnimeList.Add("123");
            AnimeList.Add("123");
            AnimeList.Add("123");
            AnimeList.Add("123");
            AnimeList.Add("123"); 
            AnimeList.Add("123");
            AnimeList.Add("123");

            NextPageCommand = ReactiveCommand.Create(NextPage);
            PrevPageCommand = ReactiveCommand.Create(PrevPage);
        }

        public void PrevPage()
        {

        }
        public void NextPage()
        {

        }

        private void ReloadAnimes()
        {
            Task.Run(() =>
            {
                LoadingAnimes = true;
                CurrentPage = 0;

                AnimeList.Clear();

            }).GetAwaiter().OnCompleted(() =>
            {
                // MessageBox.Show("the task completed in the main thread", "");

                LoadingAnimes = false;
            });
        }
    }
}
