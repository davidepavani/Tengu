using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;

namespace Tengu.Models
{
    public class GenresModel : ReactiveObject
    {
        private bool isChecked = false;

        public Genres Genre { get; set; }
        public bool IsChecked
        {
            get => isChecked;
            set => this.RaiseAndSetIfChanged(ref isChecked, value);
        }

        public GenresModel(Genres genre)
        {
            Genre = genre;
        }
    }
}
