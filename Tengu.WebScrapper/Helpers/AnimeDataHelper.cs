using HtmlAgilityPack;
using Newtonsoft.Json;
using System;

namespace Tengu.WebScrapper.Helpers
{
    public static class AnimeDataHelper
    {
        public static string GetAnimeCardFromVideoUrl(string link_video)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(link_video);

            return htmlDoc.DocumentNode.SelectSingleNode("//div/div[@class='card-body']")
                                       .SelectNodes("./a")[1]
                                       .GetAttributeValue("href", string.Empty);
        }

        public static string GetRealVideoUrl(string link_video)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(link_video);

            return htmlDoc.DocumentNode.SelectSingleNode("//div/div[@class='card-body']")
                                       .SelectNodes("./a")[0]
                                       .GetAttributeValue("href", string.Empty);
        }

        public static string ConvertToJson(object source)
        {
            return JsonConvert.SerializeObject(source);
        }
    }
}
