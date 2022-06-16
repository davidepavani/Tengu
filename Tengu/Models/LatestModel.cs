using Avalonia.Controls;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Business.Commons;
using Tengu.Extensions;

namespace Tengu.Models
{
    public class LatestModel
    {
        public EpisodeModel Episode { get; set; }
        public IControl Image { get; set; }

        public LatestModel(EpisodeModel ep)
        {
            Episode = ep;
            Image = ep.Image.InitializeImage();
        }
    }
}
