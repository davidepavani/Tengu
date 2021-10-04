using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.KitsuAPI.Anime
{
    public class DestinationModel
    {
        public DestinationLinks links { get; set; }
        public DestinationData data { get; set; }
    }

    public class DestinationLinks
    {
        public string self { get; set; }
        public string related { get; set; }
    }

    public class DestinationData
    {
        public string type { get; set; }
        public string id { get; set; }
    }
}
