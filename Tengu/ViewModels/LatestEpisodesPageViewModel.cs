using Avalonia.Collections;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Extensions;
using Tengu.Utilities;

namespace Tengu.ViewModels
{
    public class LatestEpisodesPageViewModel : ReactiveObject
    {
        private readonly ITenguApi tenguApi;

        private AvaloniaList<AnimeModel> animeList = new(); // MODEL
        private Hosts selectedHost = Hosts.AnimeSaturn; 
        private bool loadingAnimes = false;
        private int currentPage = 0;

        private bool canPrev = false;

        #region Properties
        public ICommand NextPageCommand { get; private set; }
        public ICommand PrevPageCommand { get; private set; }
        public List<Hosts> HostList { get; private set; }
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
        public AvaloniaList<AnimeModel> AnimeList
        {
            get => animeList;
            set => this.RaiseAndSetIfChanged(ref animeList, value);
        }
        public Hosts SelectedHost
        {
            get => selectedHost;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedHost, value);

                ReloadAnimes();
            }
        }
        #endregion

        public LatestEpisodesPageViewModel()
        {
            HostList = EnumExtension.ToList<Hosts>();
            tenguApi = Locator.Current.GetService<ITenguApi>();

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
