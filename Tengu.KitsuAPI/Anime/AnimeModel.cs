using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;
using Tengu.Shared.Utilities;

namespace Tengu.KitsuAPI.Anime
{
    #region AnimeByNameModel
    public class AnimeByNameModel : NotifyPropertyChanged, IAnimeByName
    {
        private List<AnimeDataModel> _data;
        [JsonProperty("data")]
        public List<AnimeDataModel> Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data"));
            }
        }
        private LinksModel _links;
        [JsonProperty("links")]
        public LinksModel Links
        {
            get { return _links; }
            set
            {
                _links = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Links"));
            }
        }
        private MetaModel _meta;
        [JsonProperty("meta")]
        public MetaModel Meta
        {
            get { return _meta; }
            set
            {
                _meta = value;
                OnPropertyChanged(new PropertyChangedEventArgs("meta"));
            }
        }
    }
    #endregion

    #region AnimeByIdModel
    public class AnimeByIdModel : NotifyPropertyChanged, IAnimeById
    {
        private AnimeDataModel _data;
        [JsonProperty("data")]
        public AnimeDataModel Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Data"));
            }
        }
        
        [JsonProperty("errors")]
        public AnimeError[] Errors { get; private set; } = { };
    }
    #endregion

    #region AnimeDataModel
    public class AnimeDataModel : NotifyPropertyChanged, IAnimeData
    {
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

        private AnimeAttributesModel _attributes;
        [JsonProperty("attributes")]
        public AnimeAttributesModel Attributes
        {
            get { return _attributes; }
            set
            {
                _attributes = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Attributes"));
            }
        }
    }
    #endregion

    #region AnimeAttributesModel
    public class AnimeAttributesModel : NotifyPropertyChanged, IAnimeAttributes
    {
        private string _tba;
        [JsonProperty("tba")]
        public string Tba
        {
            get { return _tba; }
            set
            {
                _tba = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tba"));
            }
        }

        private string[] _abbreviate_titles;
        [JsonProperty("abbreviatedTitles")]
        public string[] AbbreviatedTitles
        {
            get { return _abbreviate_titles; }
            set
            {
                _abbreviate_titles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AbbreviatedTitles"));
            }
        }

        private string _average_rating;
        [JsonProperty("averageRating")]
        public string AverageRating
        {
            get { return _average_rating; }
            set
            {
                _average_rating = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AverageRating"));
            }
        }

        private string _status;
        [JsonProperty("status")]
        public string Status
        {
            get { return _status; }
            set
            {
                _status = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Status"));
            }
        }

        private string _age_rating;
        [JsonProperty("ageRating")]
        public string AgeRating
        {
            get { return _age_rating; }
            set
            {
                _age_rating = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AgeRating"));
            }
        }

        private string _subtype;
        [JsonProperty("subtype")]
        public string Subtype
        {
            get { return _subtype; }
            set
            {
                _subtype = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Subtype"));
            }
        }

        private string _canonical_title;
        [JsonProperty("canonicalTitle")]
        public string CanonicalTitle
        {
            get { return _canonical_title; }
            set
            {
                _canonical_title = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CanonicalTitle"));
            }
        }

        private string _episode_length;
        [JsonProperty("episodeLength")]
        public string EpisodeLength
        {
            get { return _episode_length; }
            set
            {
                _episode_length = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EpisodeLength"));
            }
        }

        private string _youtube_video_id;
        [JsonProperty("youtubeVideoId")]
        public string YoutubeVideoId
        {
            get { return _youtube_video_id; }
            set
            {
                _youtube_video_id = value;
                OnPropertyChanged(new PropertyChangedEventArgs("YoutubeVideoId"));
            }
        }

        private AnimeCoverImageModel _cover_image;
        [JsonProperty("coverImage")]
        public AnimeCoverImageModel CoverImage
        {
            get { return _cover_image; }
            set
            {
                _cover_image = value;
                OnPropertyChanged(new PropertyChangedEventArgs("CoverImage"));
            }
        }

        private string _slug;
        [JsonProperty("slug")]
        public string Slug
        {
            get { return _slug; }
            set
            {
                _slug = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Slug"));
            }
        }

        private AnimeTitlesModel _titles;
        [JsonProperty("titles")]
        public AnimeTitlesModel Titles
        {
            get { return _titles; }
            set
            {
                _titles = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Titles"));
            }
        }

        private string _age_rating_guide;
        [JsonProperty("ageRatingGuide")]
        public string AgeRatingGuide
        {
            get { return _age_rating_guide; }
            set
            {
                _age_rating_guide = value;
                OnPropertyChanged(new PropertyChangedEventArgs("AgeRatingGuide"));
            }
        }

        private string _start_date;
        [JsonProperty("startDate")]
        public string StartDate
        {
            get { return _start_date; }
            set
            {
                _start_date = value;
                OnPropertyChanged(new PropertyChangedEventArgs("StartDate"));
            }
        }

        private string _episode_count;
        [JsonProperty("episodeCount")]
        public string EpisodeCount
        {
            get { return _episode_count; }
            set
            {
                _episode_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EpisodeCount"));
            }
        }

        private string _favorites_count;
        [JsonProperty("favoritesCount")]
        public string FavoritesCount
        {
            get { return _favorites_count; }
            set
            {
                _favorites_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("FavoritesCount"));
            }
        }

        private bool _nsfw;
        [JsonProperty("nsfw")]
        public bool Nsfw
        {
            get { return _nsfw; }
            set
            {
                _nsfw = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Nsfw"));
            }
        }

        private string _end_date;
        [JsonProperty("endDate")]
        public string EndDate
        {
            get { return _end_date; }
            set
            {
                _end_date = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EndDate"));
            }
        }

        private string _rating_rank;
        [JsonProperty("ratingRank")]
        public string RatingRank
        {
            get { return _rating_rank; }
            set
            {
                _rating_rank = value;
                OnPropertyChanged(new PropertyChangedEventArgs("RatingRank"));
            }
        }

        private AnimePosterImageModel _poster_image;
        [JsonProperty("posterImage")]
        public AnimePosterImageModel PosterImage
        {
            get { return _poster_image; }
            set
            {
                _poster_image = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PosterImage"));
            }
        }

        private string _synopsis;
        [JsonProperty("synopsis")]
        public string Synopsis
        {
            get { return _synopsis; }
            set
            {
                _synopsis = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Synopsis"));
            }
        }

        private string _show_type;
        [JsonProperty("showType")]
        public string ShowType
        {
            get { return _show_type; }
            set
            {
                _show_type = value;
                OnPropertyChanged(new PropertyChangedEventArgs("ShowType"));
            }
        }

        private string _user_count;
        [JsonProperty("userCount")]
        public string UserCount
        {
            get { return _user_count; }
            set
            {
                _user_count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("UserCount"));
            }
        }

        private string _popularity_rank;
        [JsonProperty("popularityRank")]
        public string PopularityRank
        {
            get { return _popularity_rank; }
            set
            {
                _popularity_rank = value;
                OnPropertyChanged(new PropertyChangedEventArgs("PopularityRank"));
            }
        }
    }
    #endregion

    #region CoverImageModel
    public class AnimeCoverImageModel : NotifyPropertyChanged, IAnimeCoverImage
    {
        private string _original;
        [JsonProperty("original")]
        public string Original
        {
            get { return _original; }
            set
            {
                _original = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Original"));
            }
        }

        private string _tiny;
        [JsonProperty("tiny")]
        public string Tiny
        {
            get { return _tiny; }
            set
            {
                _tiny = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tiny"));
            }
        }

        private string _small;
        [JsonProperty("small")]
        public string Small
        {
            get { return _small; }
            set
            {
                _small = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Small"));
            }
        }

        private string _large;
        [JsonProperty("large")]
        public string Large
        {
            get { return _large; }
            set
            {
                _large = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Large"));
            }
        }
    }
    #endregion

    #region TitlesModel
    public class AnimeTitlesModel : NotifyPropertyChanged, IAnimeTitles
    {
        private string _enjp;
        [JsonProperty("en_jp")]
        public string EnJp
        {
            get { return _enjp; }
            set
            {
                _enjp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("EnJp"));
            }
        }

        private string _jajp;
        [JsonProperty("ja_jp")]
        public string JaJp
        {
            get { return _jajp; }
            set
            {
                _jajp = value;
                OnPropertyChanged(new PropertyChangedEventArgs("JaJp"));
            }
        }
    }
    #endregion

    #region PosterImageModel
    public class AnimePosterImageModel : NotifyPropertyChanged, IAnimePosterImage
    {
        private string _tiny;
        [JsonProperty("tiny")]
        public string Tiny
        {
            get { return _tiny; }
            set 
            { 
                _tiny = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Tiny"));
            }
        }

        private string _small;
        [JsonProperty("small")]
        public string Small
        {
            get { return _small; }
            set
            {
                _small = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Small"));
            }
        }

        private string _medium;
        [JsonProperty("medium")]
        public string Medium
        {
            get { return _medium; }
            set 
            { 
                _medium = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Medium"));
            }
        }

        private string _large;
        [JsonProperty("large")]
        public string Large
        {
            get { return _large; }
            set
            {
                _large = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Large"));
            }
        }

        private string _original;
        [JsonProperty("original")]
        public string Original
        {
            get { return _original; }
            set
            {
                _original = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Original"));
            }
        }
    }
    #endregion

    #region LinksModel
    public class LinksModel : NotifyPropertyChanged
    {
        private string _first;
        [JsonProperty("first")]
        public string First
        {
            get { return _first; }
            set
            {
                _first = value;
                OnPropertyChanged(new PropertyChangedEventArgs("First"));
            }
        }
        private string _prev;
        [JsonProperty("prev")]
        public string Prev
        {
            get { return _prev; }
            set
            {
                _prev = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Prev"));
            }
        }
        private string _next;
        [JsonProperty("next")]
        public string Next
        {
            get { return _next; }
            set
            {
                _next = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Next"));
            }
        }
        private string _last;
        [JsonProperty("last")]
        public string Last
        {
            get { return _last; }
            set
            {
                _last = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Last"));
            }
        }
    }
    #endregion

    #region MetaModel
    public class MetaModel : NotifyPropertyChanged
    {
        private int _count;
        [JsonProperty("count")]
        public int Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Count"));
            }
        }
    }
    #endregion

    #region AnimeError
    public class AnimeError : IAnimeError
    {
        [JsonProperty("title")]
        public string Title { get; private set; }
        
        [JsonProperty("detail")]
        public string Detail { get; private set; }
        
        [JsonProperty("code")]
        public string Code { get; private set; }
        
        [JsonProperty("status")]
        public string Status { get; private set; }
    }
    #endregion
}