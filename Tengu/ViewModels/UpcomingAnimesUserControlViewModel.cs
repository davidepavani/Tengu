using HandyControl.Tools;
using HandyControl.Tools.Command;
using NLog;
using Notification.Wpf;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.KitsuAPI;
using Tengu.KitsuAPI.Anime;
using Logger = NLog.Logger;

namespace Tengu.ViewModels
{
    public class UpcomingAnimesUserControlViewModel : BindablePropertyBase
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private ObservableCollection<AnimeDataModel> upcoming_list;

        private readonly IRegionManager _regionManager;

        private bool is_loading;

        #region Properties
        public ICommand ShowAnimeCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }
        public ObservableCollection<AnimeDataModel> UpcomingList
        {
            get { return upcoming_list; }
            set
            {
                upcoming_list = value;
                RaisePropertyChanged();
            }
        }
        public bool IsLoading
        {
            get { return is_loading; }
            set
            {
                is_loading = value;
                RaisePropertyChanged();
            }
        }
        #endregion

        public UpcomingAnimesUserControlViewModel(IRegionManager regionManager) : base()
        {
            _regionManager = regionManager;

            UpcomingList = new ObservableCollection<AnimeDataModel>();
            IsLoading = false;

            ShowAnimeCommand = new RelayCommand<AnimeDataModel>(ShowAnimeCard);
            RefreshCommand = new SimpleRelayCommand(RefreshList);

            RefreshList();
        }
        public UpcomingAnimesUserControlViewModel() { }

        private void ShowAnimeCard(AnimeDataModel kitsu_anime)
        {
            // Define navigation parameters
            NavigationParameters navigationParams = new();
            navigationParams.Add("KitsuAnimeModel", kitsu_anime);
            navigationParams.Add("KitsuAnimeID", null);

            log.Info("ShowAnimeCard >> 'KitsuAnimeModel' : " + kitsu_anime.Attributes.CanonicalTitle);

            _regionManager.RequestNavigate("ControlsRegion", "KitsuAnimeCardUserControl", navigationParams);
        }

        private void RefreshList()
        {
            if (!IsLoading)
            {
                IsLoading = true;
                UpcomingList.Clear();

                AsyncGetUpcomingAnime();
            }
        }

        public async void AsyncGetUpcomingAnime()
        {
            AnimeByNameModel list = await Anime.GetUpcomingAnimeAsync();

            DispatcherHelper.RunOnMainThread(() =>
            {
                UpcomingList.AddRange(list.Data);
                IsLoading = false;
            });
        }
    }
}
