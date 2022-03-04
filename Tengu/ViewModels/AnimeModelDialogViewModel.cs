using Avalonia.Collections;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API;
using Tengu.Business.Commons;

namespace Tengu.ViewModels
{
    public class AnimeModelDialogViewModel : ReactiveObject
    {
        private readonly ITenguApi tenguApi;

        private AvaloniaList<EpisodeModel> episodesList = new();
        private bool loading;

        public AnimeModel AnimeData { get; private set; }
        public Hosts Host { get; private set; }
        public AvaloniaList<EpisodeModel> EpisodesList
        {
            get => episodesList;
            set => this.RaiseAndSetIfChanged(ref episodesList, value);
        }
        public bool Loading
        {
            get => loading;
            set => this.RaiseAndSetIfChanged(ref loading, value);
        }

        public AnimeModelDialogViewModel(AnimeModel anime, Hosts currentHost) : base()
        {
            tenguApi = Locator.Current.GetService<ITenguApi>();

            AnimeData = anime;
            Host = currentHost;

            Task.Run(() => LoadEpisodes());
        }
        public AnimeModelDialogViewModel() { }

        private async Task LoadEpisodes()
        {
            Loading = true;

            EpisodesList.Clear();

            try
            {
                foreach(EpisodeModel ep in await tenguApi.GetEpisodesAsync(AnimeData.Id, Host))
                {
                    EpisodesList.Add(ep);
                }
            }
            catch(Exception ex)
            {
                // Logging ..
            }
            finally
            {
                Loading = false;
            }
        }
    }
}
