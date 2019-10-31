using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using HtmlAgilityPack;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils.HtmlSource.Extension;

namespace vod.Domain.Services.Utils.HtmlSource
{
    public class HtmlSourceGetter : IHtmlSourceGetter
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly Random _random;

        public HtmlSourceGetter(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            _random = new Random();
        }

        public HtmlDocument GetHtmlFrom(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");

            Thread.Sleep(_random.Next(2000, 5000));

            var response = client.SendAsync(request).Result;
            var htmlString = response.Content.ReadAsStringAsync().Result.FixPlLetters();
            var html = new HtmlDocument();
            html.LoadHtml(htmlString);
            return html;
        }

        public void SerializeFilmDetails(HtmlDocument html, Movie movie)
        {
            var origTitle = html.DocumentNode.Descendants()
                    .FirstOrDefault(n => n.Name == "li"
                    && n.Attributes.Contains("class")
                    && n.Attributes["class"].Value == "asset-page__meta-list"
                    && n.Descendants().Any(p => p.Name == "span"
                    && p.Attributes.Contains("class")
                    && p.Attributes["class"].Value == "asset-page__meta-label"
                    && p.InnerText == "Tytuł oryginalny"))
                    .InnerText.Replace("\n", "").Replace("Tytuł oryginalny", string.Empty);

            var director = html.DocumentNode.Descendants()
                    .FirstOrDefault(n => n.Name == "li"
                    && n.Attributes.Contains("class")
                    && n.Attributes["class"].Value == "asset-page__meta-list"
                    && n.Descendants().Any(p => p.Name == "span"
                    && p.Attributes.Contains("class")
                    && p.Attributes["class"].Value == "asset-page__meta-label"
                    && p.InnerText == "Reżyseria"))
                    .InnerText.Replace("\n", "").Replace("Reżyseria", string.Empty);

            movie.OriginalTitle = origTitle
                                    .Replace(" hd", "")
                                    .Replace(" HD", "")
                                    .Replace(" sd", "")
                                    .Replace(" SD", "");
            movie.Director = director;
        }
    }
}
