using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Shared.Utilities;

namespace Tengu.KitsuAPI.Anime
{
    #region GenresModel
    public class GenresModel : NotifyPropertyChanged
    {
        private List<GenresData> _data;
        [JsonProperty("data")]
        public List<GenresData> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data"));
            }
        }
    }
    #endregion

    #region GenresData
    public class GenresData : NotifyPropertyChanged
    {
        private string _type;
        [JsonProperty("type")]
        public string Type
        {
            get { return _type; }
            set
            {
                _type = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Type"));
            }
        }
        private string _id;
        [JsonProperty("id")]
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Id"));
            }
        }
    }
    #endregion

    public partial class GenresByIdModel
    {
        [JsonProperty("data")]
        public Datum[] Data { get; set; }

        [JsonProperty("meta")]
        public Meta Meta { get; set; }

        [JsonProperty("links")]
        public GenresByIdModelLinks Links { get; set; }
    }

    public partial class Datum
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("links")]
        public DatumLinks Links { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }

    public partial class Attributes
    {
        [JsonProperty("createdAt")]
        public string CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }
    }

    public partial class DatumLinks
    {
        [JsonProperty("self")]
        public Uri Self { get; set; }
    }

    public partial class GenresByIdModelLinks
    {
        [JsonProperty("first")]
        public Uri First { get; set; }

        [JsonProperty("last")]
        public Uri Last { get; set; }
    }

    public partial class Meta
    {
        [JsonProperty("count")]
        public string Count { get; set; }
    }
}
