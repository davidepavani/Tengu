using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using ReactiveUI;

namespace Tengu.ViewModels
{
    public class SearchControlViewModel : ViewModelBase
    {
        private Hosts seletctedHost = Hosts.AnimeSaturn;

        public Hosts SelectedHost
        {
            get => seletctedHost;
            set
            {
                this.RaiseAndSetIfChanged(ref seletctedHost, value);
                ProgramConfig.Hosts.Search = SelectedHost;
            }
        }

        public SearchControlViewModel()
        {
            SelectedHost = ProgramConfig.Hosts.Search;
        }
    }
}
