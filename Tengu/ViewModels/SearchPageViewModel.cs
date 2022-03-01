using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.API;
using Tengu.Business.Commons;
using Tengu.Extensions;
using Tengu.Models;

namespace Tengu.ViewModels
{
    public class SearchPageViewModel : ReactiveObject
    {
        private Hosts selectedHost = Hosts.None;
        private Statuses selectedStatus = Statuses.None;

        #region Properties
        public List<Hosts> HostList { get; private set; }
        public List<GenresModel> GenresList { get; private set; }
        public List<Statuses> StatusesList { get; private set; }
        public Hosts SelectedHost
        {
            get => selectedHost;
            set => this.RaiseAndSetIfChanged(ref selectedHost, value);
        }
        public Statuses SelectedStatus
        {
            get => selectedStatus;
            set => this.RaiseAndSetIfChanged(ref selectedStatus, value);
        }
        #endregion

        public SearchPageViewModel()
        {
            HostList = EnumExtension.ToList<Hosts>();
            GenresList = new();

            EnumExtension.ToList<Genres>().ForEach(x => GenresList.Add(new(x)));

            StatusesList = EnumExtension.ToList<Statuses>();
        }
    }
}
