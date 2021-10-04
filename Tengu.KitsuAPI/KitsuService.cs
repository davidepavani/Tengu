using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Tengu.KitsuAPI
{
    public class KitsuService
    {
        private static readonly string UserAgent = "Tengu/v01";

        internal const string BaseUri = "https://kitsu.io/api/edge";
        internal const string BaseAuthUri = "https://kitsu.io/api/oauth";

        private static HttpClient RequestClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.api+json"));
            client.DefaultRequestHeaders.Add("User-Agent", UserAgent);
            return client;
        }

        internal static readonly HttpClient Client = RequestClient();
    }
}
