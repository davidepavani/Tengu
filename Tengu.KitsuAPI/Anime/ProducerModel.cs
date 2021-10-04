using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.KitsuAPI.Anime
{
    public class ProducerAttributes
    {
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string slug { get; set; }
        public string name { get; set; }
    }

    public class AnimeProductions
    {
        public Links links { get; set; }
    }

    public class Productions
    {
        public Links links { get; set; }
    }

    public class ProducerRelationships
    {
        public AnimeProductions animeProductions { get; set; }
        public Productions productions { get; set; }
    }

    public class ProducerData
    {
        public string id { get; set; }
        public string type { get; set; }
        public Links links { get; set; }
        public ProducerAttributes attributes { get; set; }
        public ProducerRelationships relationships { get; set; }
    }

    public class ProducerModel
    {
        public ProducerData data { get; set; }
    }
}
