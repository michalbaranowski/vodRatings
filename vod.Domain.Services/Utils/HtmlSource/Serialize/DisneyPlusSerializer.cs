using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Utils.HtmlSource.Serialize
{
    public class DisneyPlusSerializer : IDisneyPlusSerializer
    {
        private const string ProviderName = "DisneyPlus";

        public IEnumerable<string> SerializeUrls(string sitemapHtml)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(sitemapHtml);

            var hrefs = htmlDoc.DocumentNode
                .Descendants("a")
                .Where(n => n.Attributes.Contains("class") && n.Attributes.Contains("href") && n.Attributes["class"].Value == "skip-link-style style-base ada-el-focus")
                .Select(n => n.Attributes["href"].Value);

            return hrefs;
        }

        public FilmResultWithMovieType Serialize(string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var movieName = htmlDoc.DocumentNode
                .Descendants("h1")
                .Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value == "catalog-title")
                .First().InnerText;

            var movieTypes = htmlDoc.DocumentNode
                .Descendants("div")
                .Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value == "meta-title flex-meta-item" && n.Descendants("p").Count() > 0)
                .FirstOrDefault().InnerText;

            var movieType = MovieTypes.Action;

            if (movieTypes.Contains("Thriller"))
            {
                movieType = MovieTypes.Thriller;
            }
            else if (movieTypes.Contains("Family"))
            {
                movieType = MovieTypes.Cartoons;
            }

            return new FilmResultWithMovieType()
            {
                ProviderName = ProviderName,
                Title = movieName,
                MovieType = movieType
            };
        }
    }
}
