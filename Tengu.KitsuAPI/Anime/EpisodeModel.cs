using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tengu.KitsuAPI.Anime
{
    public partial class EpisodesByIdModel
    {
        [JsonProperty("data")]
        public List<DatumEp> Data { get; set; }

        [JsonProperty("meta")]
        public EpisodesByIdModelMeta Meta { get; set; }

        [JsonProperty("links")]
        public EpisodesByIdModelLinks Links { get; set; }
    }

    public partial class DatumEp
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("links")]
        public DatumLinksEp Links { get; set; }

        [JsonProperty("attributes")]
        public AttributesEp Attributes { get; set; }

        [JsonProperty("relationships")]
        public Relationships Relationships { get; set; }

        public override string ToString()
        {
            string to_string = this.Attributes.Number;

            if(!string.IsNullOrEmpty(this.Attributes.CanonicalTitle))
            {
                to_string += " - " + this.Attributes.CanonicalTitle;
            }
            else if(this.Attributes.Titles != null)
            {
                if (!string.IsNullOrEmpty(this.Attributes.Titles.EnUs))
                {
                    to_string += " - " + this.Attributes.Titles.EnUs;
                }
                else if (!string.IsNullOrEmpty(this.Attributes.Titles.JaJp))
                {
                    to_string += " - " + this.Attributes.Titles.JaJp;
                }
            }

            return to_string;
        }
    }

    public partial class AttributesEp
    {
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("synopsis")]
        public string Synopsis { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("titles")]
        public Titles Titles { get; set; }

        [JsonProperty("canonicalTitle")]
        public string CanonicalTitle { get; set; }

        [JsonProperty("seasonNumber")]
        public string SeasonNumber { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("relativeNumber")]
        public string RelativeNumber { get; set; }

        [JsonProperty("airdate")]
        public string Airdate { get; set; }

        [JsonProperty("length")]
        public string Length { get; set; }

        [JsonProperty("thumbnail")]
        public Thumbnail Thumbnail { get; set; }
    }

    public partial class Thumbnail
    {
        [JsonProperty("original")]
        public string Original { get; set; }

        [JsonProperty("meta")]
        public ThumbnailMeta Meta { get; set; }
    }

    public partial class ThumbnailMeta
    {
        [JsonProperty("dimensions")]
        public Dimensions Dimensions { get; set; }
    }

    public partial class Dimensions
    {
    }

    public partial class Titles
    {
        [JsonProperty("en_jp")]
        public string EnJp { get; set; }

        [JsonProperty("en_us")]
        public string EnUs { get; set; }

        [JsonProperty("ja_jp")]
        public string JaJp { get; set; }
    }

    public partial class DatumLinksEp
    {
        [JsonProperty("self")]
        public string Self { get; set; }
    }

    public partial class Relationships
    {
        [JsonProperty("media")]
        public Media Media { get; set; }

        [JsonProperty("videos")]
        public Media Videos { get; set; }
    }

    public partial class Media
    {
        [JsonProperty("links")]
        public MediaLinks Links { get; set; }
    }

    public partial class MediaLinks
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("related")]
        public string Related { get; set; }
    }

    public partial class EpisodesByIdModelLinks
    {
        [JsonProperty("first")]
        public string First { get; set; }

        [JsonProperty("next")]
        public string Next { get; set; }

        [JsonProperty("last")]
        public string Last { get; set; }
    }

    public partial class EpisodesByIdModelMeta
    {
        [JsonProperty("count")]
        public string Count { get; set; }
    }
}


