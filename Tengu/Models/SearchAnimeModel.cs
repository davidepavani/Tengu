using Avalonia.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Avalonia.Media;
using Tengu.Extensions;
using Tengu.Business.Commons.Models;

namespace Tengu.Models
{
    public class SearchAnimeModel
    {
        public AnimeModel Anime { get; set; }
        public IControl Image { get; set; }

        public SearchAnimeModel(AnimeModel anime)
        {
            Anime = anime;
            Image = anime.Image.InitializeImage(Stretch.UniformToFill);
        }
    }
}
