﻿using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Business.Commons.Objects;

namespace Tengu.Models
{
    public class GenresModel : ReactiveObject
    {
        private bool isChecked = false;

        public TenguGenres Genre { get; set; }
        public bool IsChecked
        {
            get => isChecked;
            set => this.RaiseAndSetIfChanged(ref isChecked, value);
        }

        public GenresModel(TenguGenres genre)
        {
            Genre = genre;
        }
    }
}
