using System.Collections.Generic;
// ReSharper disable UnusedMemberInSuper.Global

namespace Tengu.KitsuAPI.Anime
{
    public interface IAnimeByName
    {
        List<AnimeDataModel> Data { get; }
    }
    
    public interface IAnimeById
    {
        AnimeDataModel Data { get; }
        AnimeError[] Errors { get; }
    }
    
    public interface IAnimeData
    {
        string Id { get; }
        string Type { get; }
        AnimeAttributesModel Attributes { get; }
    }

    public interface IAnimeAttributes
    {
        string Tba { get; }
        string[] AbbreviatedTitles { get; }
        string AverageRating { get; }
        string Status { get; }
        string AgeRating { get; }
        string Subtype { get; }
        string CanonicalTitle { get; }
        string EpisodeLength { get; }
        AnimeCoverImageModel CoverImage { get; }
        AnimeTitlesModel Titles { get; }
        string AgeRatingGuide { get; }
        string StartDate { get; }
        string EpisodeCount { get; }
        string FavoritesCount { get; }
        bool Nsfw { get; }
        string EndDate { get; }
        string RatingRank { get; }
        AnimePosterImageModel PosterImage { get; }
        string Synopsis { get; }
        string ShowType { get; }
        string UserCount { get; }
        string PopularityRank { get; }
    }

    public interface IAnimeCoverImage
    {
        string Original { get; }
        string Tiny { get; }
        string Small { get; }
        string Large { get; }
    }
    
    public interface IAnimeTitles
    {
        string EnJp { get; }
        string JaJp { get; }
    }
    
    public interface IAnimePosterImage
    {
        string Tiny { get; }
        string Small { get; }
        string Medium { get; }
        string Large { get; }
        string Original { get; }
    }

    public interface IAnimeError
    {
        string Title { get; }
        string Detail { get; }
        string Code { get; }
        string Status { get; }
    }
}