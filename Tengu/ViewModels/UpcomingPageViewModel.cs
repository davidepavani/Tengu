using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Tengu.ViewModels
{
    public class UpcomingPageViewModel : ReactiveObject
    {
        private bool loadingAnimes = false;
        private int currentPage = 0;

        private bool canPrev = false;

        #region Properties
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
        #endregion

        public UpcomingPageViewModel()
        {

        }
    }
}
