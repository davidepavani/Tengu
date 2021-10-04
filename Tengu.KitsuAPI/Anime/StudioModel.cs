using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.KitsuAPI.Anime
{
    public class StudioAttributes
    {
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string role { get; set; }
    }

    public class StudioAnime
    {
        public Links links { get; set; }
    }

    public class Producer
    {
        public Links links { get; set; }
    }

    public class StudioRelationships
    {
        public StudioAnime anime { get; set; }
        public Producer producer { get; set; }
    }

    public class StudioData
    {
        public string id { get; set; }
        public string type { get; set; }
        public Links links { get; set; }
        public StudioAttributes attributes { get; set; }
        public StudioRelationships relationships { get; set; }
    }

    public class StudioModel
    {
        public List<StudioData> data { get; set; }
        public Meta meta { get; set; }
        public Links links { get; set; }
    }
}
