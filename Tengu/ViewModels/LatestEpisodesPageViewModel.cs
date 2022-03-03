﻿using Avalonia.Collections;
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

        private AvaloniaList<EpisodeModel> episodeList = new(); 
        private Hosts selectedHost = Hosts.AnimeSaturn; 
        private bool loadingAnimes = false;
        private int currentPage = 0;
        private int offset = 0;

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
        public AvaloniaList<EpisodeModel> EpisodeList
        {
            get => episodeList;
            set => this.RaiseAndSetIfChanged(ref episodeList, value);
        }
        public Hosts SelectedHost
        {
            get => selectedHost;
            set
            {
                this.RaiseAndSetIfChanged(ref selectedHost, value);

                Initialize(value);
            }
        }
        #endregion

        public LatestEpisodesPageViewModel()
        {
            HostList = new();
            HostList.Add(Hosts.AnimeSaturn);
            HostList.Add(Hosts.AnimeUnity);

            tenguApi = Locator.Current.GetService<ITenguApi>();

            NextPageCommand = ReactiveCommand.Create(NextPage);
            PrevPageCommand = ReactiveCommand.Create(PrevPage);

            Initialize(SelectedHost);
        }

        public void PrevPage()
        {
            if (CanPrev)
            {
                CurrentPage -= 1;
                offset -= 10;

                _ = LoadAnimes();
            }
        }
        public void NextPage()
        {
            CurrentPage += 1;
            offset += 10;

            _ = LoadAnimes();
        }

        private void Initialize(Hosts host)
        {
            tenguApi.CurrentHosts = new Hosts[] { host };
            CurrentPage = 0;
            offset = 0;

            _ = LoadAnimes();
        }

        private async Task LoadAnimes()
        {
            LoadingAnimes = true;
            EpisodeList.Clear();

            foreach (EpisodeModel episode in await tenguApi.GetLatestEpisodeAsync(offset, offset + 10))
            {
                EpisodeList.Add(episode);
            }

            CanPrev = !CurrentPage.Equals(0) && EpisodeList.Count > 0;
            LoadingAnimes = false;
        }
    }
}
