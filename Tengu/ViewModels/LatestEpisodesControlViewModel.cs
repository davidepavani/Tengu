using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.ViewModels
{
    public class LatestEpisodesControlViewModel : ViewModelBase
    {
        public List<string> Episodes { get; set; } = new()
        {
            "Episode1",
            "Episode2",
            "Episode3",
            "Episode4",
        };
    }
}
