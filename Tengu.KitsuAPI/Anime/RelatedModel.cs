using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.KitsuAPI.Anime
{
    public class RelatedModel
    {
        [JsonProperty("data")]
        public List<RelatedData> Data { get; set; }

        [JsonProperty("meta")]
        public RelatedMeta Meta { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }
    }

    public class Links
    {
        public string self { get; set; }
        public string related { get; set; }
        public string next { get; set; }
        public string first { get; set; }
        public string last { get; set; }
    }

    public class RelatedAttributes
    {
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
        public string role { get; set; }
    }

    public class Source
    {
        public Links links { get; set; }
    }

    public class Destination
    {
        public Links links { get; set; }
    }

    public class RelatedRelationships
    {
        public Source source { get; set; }
        public Destination destination { get; set; }
    }

    public class RelatedData
    {
        public string id { get; set; }
        public string type { get; set; }
        public Links links { get; set; }
        public RelatedAttributes attributes { get; set; }
        public RelatedRelationships relationships { get; set; }
    }

    public class RelatedMeta
    {
        public int count { get; set; }
    }
}
