using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Enums;
using Tengu.Extensions;

namespace Tengu.ViewModels
{
    public class LatestEpisodesPageViewModel : ReactiveObject
    {
        private TenguHost selectedHost;
        private bool loadingAnimes;

        #region Properties
        public List<TenguHost> HostList { get; private set; }
        public bool LoadingAnimes
        {
            get => loadingAnimes;
            set => this.RaiseAndSetIfChanged(ref loadingAnimes, value);
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
            LoadingAnimes = false;
            SelectedHost = TenguHost.AnimeSaturn;
        }

        private void ReloadAnimes()
        {
            loadingAnimes = true;
        }
    }
}
