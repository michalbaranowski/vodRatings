using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils.HtmlSource.Extension;

namespace vod.Domain.Services.Utils.HtmlSource.Serialize
{
    public class HtmlSourceSerializer : IHtmlSourceSerializer
    {
        public IEnumerable<Movie> SerializeMovies(HtmlDocument html, MovieTypes type)
        {
            var divs = html.DocumentNode.Descendants("div")
                .Where(x => x.Attributes.Contains("title"));

            var providerNode = html.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "h2" && n.Attributes.Contains("class") && n.Attributes["class"].Value == "popup__title");
            var provider = providerNode?.InnerHtml.Split("\n")[0].Trim();

            return divs.Select(n =>
                    new Movie()
                    {
                        Title = n.Attributes?
                            .FirstOrDefault(p => p.Name == "title")?
                            .Value
                            .Replace("(HD)", string.Empty)
                            .Replace("(SD)", ""),
                        ProviderName = provider,
                        MoreInfoUrl = n.Descendants().Where(x=>x.Name == "a" && x.Attributes.Contains("href"))?.FirstOrDefault()?.Attributes["href"].Value,
                        MovieType = type
                    });
        }

        public string SerializeFilmwebUrl(HtmlDocument html)
        {
            return html.DocumentNode.Descendants()?
                .Where(n => n.Name == "a" && n.Attributes.Contains("href") && n.Attributes["href"].Value.Contains("filmweb")).FirstOrDefault()
                ?.Attributes["href"].Value;
        }

        public FilmwebResult SerializeFilmwebResult(HtmlDocument filmwebHtml, MovieTypes movieMovieType)
        {
            var rate = filmwebHtml.DocumentNode.Descendants()?
                .FirstOrDefault(n => n.Name == "span" && n.Attributes.Contains("itemprop") &&
                            n.Attributes["itemprop"].Value.Contains("ratingValue"))?.InnerHtml;

            var rateCount = filmwebHtml.DocumentNode.Descendants()?
                .FirstOrDefault(n => n.Name == "span" && n.Attributes.Contains("itemprop") &&
                                     n.Attributes["itemprop"].Value.Contains("ratingCount"))?.InnerHtml;

            var titleH1 = filmwebHtml.DocumentNode.Descendants()
                ?.Where(n =>
                    n.Name == "h1" && n.Attributes.Contains("class") &&
                    n.Attributes["class"].Value.Contains("filmTitle")).FirstOrDefault();

            var title = titleH1?.Descendants().FirstOrDefault(n => n.Name == "a")?.Attributes["title"].Value.FixPlLetters();
            var year = titleH1?.Descendants().FirstOrDefault(n => n.Name == "span")?.InnerHtml
                .Replace("(", string.Empty).Replace(")", string.Empty);

            var imageUrl = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "img" && n.Attributes.Contains("alt") && n.Attributes.Contains("itemprop") &&
                n.Attributes["itemprop"].Value == "image")?.Attributes["src"].Value;

            var filmwebFilmType = filmwebHtml.DocumentNode.Descendants()
                .FirstOrDefault(n =>
                    n.Name == "ul" && n.Attributes.Contains("class") &&
                    n.Attributes["class"].Value.Contains("genresList"))?.Descendants().FirstOrDefault(n => n.Name == "a")?
                .InnerText;

            return new FilmwebResult()
            {
                FilmwebRating = Convert.ToDecimal(rate),
                FilmwebRatingCount  = Convert.ToInt32(rateCount),
                FilmwebFilmType = filmwebFilmType,
                Title = title,
                Year = Convert.ToInt32(year.OnlyDigits()),
                ImageUrl = imageUrl,
                VodFilmType = (int)movieMovieType
            };
        }
    }
}
