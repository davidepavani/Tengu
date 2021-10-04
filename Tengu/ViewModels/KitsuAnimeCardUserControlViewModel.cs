using HandyControl.Tools;
using HandyControl.Tools.Command;
using NLog;
using Notification.Wpf;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tengu.KitsuAPI.Anime;
using Logger = NLog.Logger;

namespace Tengu.ViewModels
{
    public class KitsuAnimeCardUserControlViewModel : BindablePropertyBase, INavigationAware
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        private const string YOUTUBE_PREFIX = "https://www.youtube.com/watch?v=";

        private readonly IRegionManager _regionManager;
        private AnimeDataModel kitsu_anime;
        private ObservableCollection<AnimeDataModel> related_list;
        private ObservableCollection<string> genres;

        private string studio;

        private bool is_loading;
        private bool loading_related;

        private string youtube_link;

        #region Properties
        public ICommand CommandGoBack { get; private set; }
        public string Studio
        {
            get { return studio; }
            set
            {
                studio = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<AnimeDataModel> RelatedList
        {
            get { return related_list; }
            set
            {
                related_list = value;
                RaisePropertyChanged();
            }
        }
        public string YoutubeLink
        {
            get { return youtube_link; }
            set
            {
                youtube_link = value;
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<string> Genres
        {
            get { return genres; }
            set
            {
                genres = value;
                RaisePropertyChanged();
            }
        }
        public AnimeDataModel KitsuAnime
        {
            get { return kitsu_anime; }
            set
            {
                kitsu_anime = value;
                RaisePropertyChanged();
            }
        }
        public bool LoadingRelated
        {
            get { return loading_related; }
            set
            {
                loading_related = value;
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

        public KitsuAnimeCardUserControlViewModel(IRegionManager regionManager) : base()
        {
            _regionManager = regionManager;

            RelatedList = new ObservableCollection<AnimeDataModel>();
            Genres = new ObservableCollection<string>();
            KitsuAnime = new AnimeDataModel();
            YoutubeLink = string.Empty;
            Studio = string.Empty;

            IsLoading = false;
            LoadingRelated = false;

            CommandGoBack = new SimpleRelayCommand(GoBack);
        }
        public KitsuAnimeCardUserControlViewModel() { }

        private bool CanGoBack()
        {
            return _regionManager.Regions["ControlsRegion"].NavigationService.Journal.CanGoBack;
        }

        private void GoBack()
        {
            if (CanGoBack())
            {
                log.Debug("KitsuAnimeCard >> GoBack Clicked");

                _regionManager.Regions["ControlsRegion"].NavigationService.Journal.GoBack();
            }
        }

        // Called from Click event and not from Command because it doesen't work from drawer
        // TODO - Understand why and solve it .. 
        public void ShowAnimeCard(string anime_id)
        {
            // Define navigation parameters
            NavigationParameters navigationParams = new();
            navigationParams.Add("KitsuAnimeModel", null);
            navigationParams.Add("KitsuAnimeID", anime_id);

            log.Info("ShowAnimeCard >> 'KitsuAnimeID' : " + anime_id);

            _regionManager.RequestNavigate("ControlsRegion", "KitsuAnimeCardUserControl", navigationParams);
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // Do nothing .. 
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            IsLoading = true;
            LoadingRelated = true;

            // Get URL from Navigation parameters
            var anime = (AnimeDataModel)navigationContext.Parameters["KitsuAnimeModel"];

            if(anime == null)
            {
                int id = Convert.ToInt32(navigationContext.Parameters["KitsuAnimeID"]);
                AsyncGetAnimeByID(id);
            }
            else
            {
                KitsuAnime = anime;
                IsLoading = false;

                if (!string.IsNullOrEmpty(KitsuAnime.Attributes.YoutubeVideoId)) 
                {
                    YoutubeLink = YOUTUBE_PREFIX + KitsuAnime.Attributes.YoutubeVideoId;
                }

                log.Info("KitsuAnimeCard >> 'KitsuAnimeModel' : " + KitsuAnime.Id);

                // Load Genres, Related and Studio
                AsyncLoadGenres();
                AsyncLoadRelated();
                AsyncLoadStudio();
            }
        }

        public async void AsyncGetAnimeByID(int id)
        {
            var temp_kitsu_anime = await Anime.GetAnimeAsync(id);

            DispatcherHelper.RunOnMainThread(() =>
            {
                KitsuAnime = temp_kitsu_anime.Data;
                IsLoading = false;

                if (!string.IsNullOrEmpty(KitsuAnime.Attributes.YoutubeVideoId))
                {
                    YoutubeLink = YOUTUBE_PREFIX + KitsuAnime.Attributes.YoutubeVideoId;
                }
            });

            log.Info("KitsuAnimeCard >> 'KitsuAnimeModel' : " + KitsuAnime.Id);

            // Load Genres, Related and Studio
            AsyncLoadGenres();
            AsyncLoadRelated();
            AsyncLoadStudio();
        }

        private async void AsyncLoadGenres()
        {
            Genres.Clear();

            var genres = await Anime.GetAnimeGenresById(Convert.ToInt32(KitsuAnime.Id));

            DispatcherHelper.RunOnMainThread(() =>
            {
                foreach (Datum gen in genres.Data)
                {
                    Genres.Add(gen.Attributes.Name);
                }

                if (Genres.Count == 0)
                {
                    Genres.Add("No Genres found");
                }
            });
        }

        private async void AsyncLoadRelated()
        {
            RelatedList.Clear();

            var related = await Anime.GetAnimeRelatedById(Convert.ToInt32(KitsuAnime.Id));

            DispatcherHelper.RunOnMainThread(() =>
            {
                RelatedList.AddRange(related.Data);

                LoadingRelated = false;
            });
        }

        private async void AsyncLoadStudio()
        {
            var _studio = await Anime.GetAnimeStudio(Convert.ToInt32(KitsuAnime.Id));

            DispatcherHelper.RunOnMainThread(() =>
            {
                Studio = _studio;
            });
        }
    }
}
