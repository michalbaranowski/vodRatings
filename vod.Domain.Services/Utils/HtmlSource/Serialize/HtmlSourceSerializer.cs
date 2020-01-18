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

        public string SerializeFilmwebUrl(HtmlDocument html, List<string> directors)
        {
            var ul = html.DocumentNode.Descendants().FirstOrDefault(n => n.Name == "ul" && n.Attributes.Contains("class") && n.Attributes["class"].Value == "resultsList hits");
            if (ul == null) return string.Empty;

            var li = ul.Descendants()
                .FirstOrDefault(n=>n.Name == "li" 
                                && n.HasChildNodes 
                                && n.Descendants().Any(p=>
                                    p.Name == "div" 
                                    && p.Attributes.Contains("class") 
                                    && p.Attributes["class"].Value == "filmPreview__info filmPreview__info--directors"
                                    && directors.Any(x=>x == p.InnerText.Replace("reżyser", ""))));

            if (li == null) return string.Empty;

            var href = li.Descendants()
                .FirstOrDefault(n => n.Name == "a" && n.Attributes.Contains("class") && n.Attributes.Contains("href") && n.Attributes["class"]?
                .Value == "filmPreview__link")?
                .Attributes["href"]?
                .Value;
            return href != null ? $"{FilmwebUrls.FilmwebBaseUrl}{href}" : string.Empty;
        }

        public FilmwebResult SerializeFilmwebResult(HtmlDocument filmwebHtml, MovieTypes movieType, string moreInfoUrl, string movieTitle)
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

            var title = titleH1?.Descendants().FirstOrDefault(n => n.Name == "a")?.Attributes["title"].Value;
            var year = titleH1?.Descendants().FirstOrDefault(n => n.Name == "span")?.InnerHtml
                .Replace("(", string.Empty).Replace(")", string.Empty);

            var imageUrl = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "img" && n.Attributes.Contains("alt") && n.Attributes.Contains("itemprop") &&
                n.Attributes["itemprop"].Value == "image")?.Attributes["src"].Value;

            var filmwebFilmTypes = filmwebHtml.DocumentNode.Descendants()
                .FirstOrDefault(n =>
                    n.Name == "ul" && n.Attributes.Contains("class") &&
                    n.Attributes["class"].Value.Contains("genresList"))?.Descendants().Where(n => n.Name == "a")
                ?.Select(n => n.InnerText);

            var filmwebFilmType = filmwebFilmTypes != null ? string.Join(", ", filmwebFilmTypes) : string.Empty;

            var production = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "a" && n.Attributes.Contains("href") && n.Attributes["href"].Value.Contains("countries"))?.InnerText;

                var filmDesc = filmwebHtml.DocumentNode.Descendants()
                    .FirstOrDefault(n =>
                        n.Name == "div" && n.Attributes.Contains("class") &&
                        n.Attributes["class"].Value.Contains("filmPlot"))?
                    .Descendants().FirstOrDefault(n => n.Name == "p")?.InnerText;

            var cast = filmwebHtml.DocumentNode.Descendants()
                .Where(n => n.Name == "tr"
                        && n.Attributes.Contains("id")
                        && n.Attributes["id"].Value.Contains("role_"))
                            .Select(n => n.Descendants()
                                    .FirstOrDefault(p => p.Name == "td")
                                        .Descendants()
                                            .FirstOrDefault(r => r.Name == "a" && r.Attributes.Contains("title")).Attributes["title"].Value).ToList();

            var ratingValue = 0m;
            decimal.TryParse(rate, out ratingValue);

            var ratingCountValue = 0;
            int.TryParse(rateCount, out ratingCountValue);

            var yearValue = 0;
            int.TryParse(year.OnlyDigits(), out yearValue);

            return new FilmwebResult()
            {
                FilmwebRating = ratingValue,
                FilmwebRatingCount  = ratingCountValue,
                FilmwebFilmType = filmwebFilmType,
                Title = movieTitle,
                FilmwebTitle = title,
                Year = yearValue,
                ImageUrl = imageUrl,
                VodFilmType = (int)movieType,
                Production = production,
                FilmDescription = filmDesc,
                MovieUrl = $"{NcPlusUrls.NcPlusGoUrl}{moreInfoUrl}",
                Cast = cast
            };
        }
    }
}
