using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tengu.Classes.DataModels;

namespace Tengu.Classes.Extensions
{
    public static class AnimeDataExtensions
    {
        public static void GetAnimeCardFromVideoUrl(this AnimeData source)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(source.LinkVideo);

            source.LinkCard = htmlDoc.DocumentNode.SelectSingleNode("//div/div[@class='card-body']")
                                                  .SelectNodes("./a")[1]
                                                  .GetAttributeValue("href", string.Empty);
        }

        public static string GetRealVideoUrl(this AnimeData source)
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument htmlDoc = web.Load(source.LinkVideo);

            return htmlDoc.DocumentNode.SelectSingleNode("//div/div[@class='card-body']")
                                       .SelectNodes("./a")[0]
                                       .GetAttributeValue("href", string.Empty);
        }

        public static string ConvertToJson(this AnimeData source)
        {
            return JsonConvert.SerializeObject(source);
        }
    }
}
