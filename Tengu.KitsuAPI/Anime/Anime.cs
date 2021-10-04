using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
// ReSharper disable UnusedMember.Global

namespace Tengu.KitsuAPI.Anime
{
    public static class Anime
    {
        /// <summary>
        /// Search for an anime with the name
        /// </summary>
        /// <param name="name">Anime name</param>
        /// <returns>List with anime data objects</returns>
        /// <exception cref="NoDataFoundException"></exception>
        public static async Task<AnimeByNameModel> GetAnimeAsync(string name)
        {
            var json = await KitsuService.Client.GetStringAsync($"{KitsuService.BaseUri}/anime?filter[text]={name}");
            var anime = JsonConvert.DeserializeObject<AnimeByNameModel>(json);
            if (anime.Data.Count <= 0) throw new NoDataFoundException($"No anime was found with the name {name}");
            return anime;
        }

        public static async Task<AnimeByNameModel> GetUpcomingAnimeAsync()
        {
            var json = await KitsuService.Client.GetStringAsync($"{KitsuService.BaseUri}/anime?filter[Status]=Upcoming&sort=popularityRank&page[limit]=20");
            var anime = JsonConvert.DeserializeObject<AnimeByNameModel>(json);

            List<AnimeDataModel> data = anime.Data;

            while (anime.Links.Next != null)
            {
                json = await KitsuService.Client.GetStringAsync(anime.Links.Next);
                anime = JsonConvert.DeserializeObject<AnimeByNameModel>(json);

                data.AddRange(anime.Data);
            }

            anime.Data = data;

            if (anime.Data.Count <= 0) throw new NoDataFoundException($"No anime was found in anime upcoming");
            return anime;
        }

        public static async Task<GenresByIdModel> GetAnimeGenresById(int anime_index)
        {
            var json = await KitsuService.Client.GetStringAsync($"{KitsuService.BaseUri}/anime/{anime_index}/genres");
            var genres = JsonConvert.DeserializeObject<GenresByIdModel>(json);

            return genres;
        }

        public static async Task<EpisodesByIdModel> GetAnimeEpisodesById(int anime_index)
        {
            EpisodesByIdModel anime_episodes = new EpisodesByIdModel();

            var json = await KitsuService.Client.GetStringAsync($"{KitsuService.BaseUri}/anime/{anime_index}/episodes");
            var episodes = JsonConvert.DeserializeObject<EpisodesByIdModel>(json);

            anime_episodes = episodes;

            if (episodes.Data.Count <= 0) throw new NoDataFoundException($"No episodes was found with the id {anime_index}");

            while (episodes.Links.Next != null)
            {
                json = await KitsuService.Client.GetStringAsync(episodes.Links.Next);
                episodes = JsonConvert.DeserializeObject<EpisodesByIdModel>(json);

                if (episodes.Data.Count > 0)
                {
                    anime_episodes.Data.AddRange(episodes.Data);
                }
            }

            return anime_episodes;
        }

        public static async Task<string> GetAnimeStudio(int anime_index)
        {
            //https://kitsu.io/api/edge/anime/6/relationships/anime-productions
            var json = await KitsuService.Client.GetStringAsync($"{KitsuService.BaseUri}/anime/{anime_index}/anime-productions?filter[role]=studio");
            StudioModel studio_list = JsonConvert.DeserializeObject<StudioModel>(json);

            if (studio_list.data.Count <= 0) return string.Empty;

            List<string> studios = new List<string>();

            foreach(StudioData data in studio_list.data)
            {
                json = await KitsuService.Client.GetStringAsync(data.relationships.producer.links.related);
                ProducerModel prod = JsonConvert.DeserializeObject<ProducerModel>(json);

                if (!string.IsNullOrEmpty(prod.data.attributes.name))
                {
                    studios.Add(prod.data.attributes.name);
                }
            }

            return string.Join(", ", studios);
        }

        public static async Task<AnimeByNameModel> GetAnimeRelatedById(int anime_index)
        {
            AnimeByNameModel related_animes = new AnimeByNameModel();
            related_animes.Data = new List<AnimeDataModel>();

            var json = await KitsuService.Client.GetStringAsync($"{KitsuService.BaseUri}/anime/{anime_index}/media-relationships");
            RelatedModel temp_related = JsonConvert.DeserializeObject<RelatedModel>(json);

            RelatedModel anime_related = temp_related;

            if (temp_related.Data.Count <= 0) return related_animes;

            while (temp_related.Links.next != null)
            {
                json = await KitsuService.Client.GetStringAsync(temp_related.Links.next);
                temp_related = JsonConvert.DeserializeObject<RelatedModel>(json);

                if (temp_related.Data.Count > 0)
                {
                    anime_related.Data.AddRange(temp_related.Data);
                }
            }

            foreach(RelatedData data in anime_related.Data)
            {
                json = await KitsuService.Client.GetStringAsync(data.relationships.destination.links.self);
                DestinationModel dest = JsonConvert.DeserializeObject<DestinationModel>(json);

                if (dest.data.type.ToLower() == "anime")
                {
                    AnimeByIdModel anime = await GetAnimeAsync(Convert.ToInt32(dest.data.id));

                    related_animes.Data.Add(anime.Data);
                }
            }

            return related_animes;
        }

        /// <summary>
        /// Search for an anime with the name and page offset
        /// </summary>
        /// <param name="name">Anime name</param>
        /// <param name="offset">Page offset</param>
        /// <returns>List with anime data objects</returns>
        /// <exception cref="NoDataFoundException"></exception>
        public static async Task<AnimeByNameModel> GetAnimeAsync(string name, int offset)
        {
            var json = await KitsuService.Client.GetStringAsync($"{KitsuService.BaseUri}/anime?filter[text]={name}&page[offset]={offset}");
            var anime = JsonConvert.DeserializeObject<AnimeByNameModel>(json);
            if (anime.Data.Count <= 0) throw new NoDataFoundException($"No anime was found with the name {name} and offset {offset}");
            return anime;
        }

        /// <summary>
        /// Search for an anime with its id
        /// </summary>
        /// <param name="id">Anime id</param>
        /// <returns>Object with anime data</returns>
        public static async Task<AnimeByIdModel> GetAnimeAsync(int id)
        {
            var json = await KitsuService.Client.GetStringAsync($"{KitsuService.BaseUri}/anime/{id}");
            var anime = JsonConvert.DeserializeObject<AnimeByIdModel>(json);
            return anime;
        }

        /// <summary>
        /// Get the trending anime
        /// </summary>
        /// <returns>List with anime data objects</returns>
        /// <exception cref="NoDataFoundException"></exception>
        public static async Task<AnimeByNameModel> GetTrendingAsync()
        {
            var json = await KitsuService.Client.GetStringAsync($"{KitsuService.BaseUri}/trending/anime");
            var trending = JsonConvert.DeserializeObject<AnimeByNameModel>(json);
            if (trending.Data.Count <= 0) throw new NoDataFoundException("Could not find any trending anime");
            return trending;
        }
    }
}