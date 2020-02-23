using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Internal;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils.HtmlSource.Extension;

namespace vod.Domain.Services.Utils.HtmlSource.Serialize
{
    public class HtmlSourceSerializer : IHtmlSourceSerializer
    {
        public IEnumerable<NcPlusResult> SerializeMovies(HtmlDocument html, MovieTypes type)
        {
            var divs = html.DocumentNode.Descendants("div")
                .Where(x => x.Attributes.Contains("title"));

            var providerNode = html.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "h2" && n.Attributes.Contains("class") && n.Attributes["class"].Value == "popup__title");
            var provider = providerNode?.InnerHtml.Split("\n")[0].Trim();

            return divs.Select(n =>
                    new NcPlusResult()
                    {
                        Title = n.Attributes?
                            .FirstOrDefault(p => p.Name == "title")?
                            .Value
                            .Replace("(HD)", string.Empty)
                            .Replace("(SD)", ""),
                        ProviderName = provider,
                        MoreInfoUrl = n.Descendants().Where(x => x.Name == "a" && x.Attributes.Contains("href"))?.FirstOrDefault()?.Attributes["href"].Value,
                        MovieType = type
                    });
        }

        public string SerializeFilmwebUrl(HtmlDocument html, List<string> directors)
        {
            var ul = html.DocumentNode.Descendants().FirstOrDefault(n => n.Name == "ul" && n.Attributes.Contains("class") && n.Attributes["class"].Value == "resultsList hits");
            if (ul == null) return string.Empty;

            var li = ul.Descendants()
                .FirstOrDefault(n => n.Name == "li"
                                && n.HasChildNodes
                                && n.Descendants().Any(p =>
                                    p.Name == "div"
                                    && p.Attributes.Contains("class")
                                    && p.Attributes["class"].Value == "filmPreview__info filmPreview__info--directors"
                                    && directors.Any(x => x == p.InnerText.Replace("reżyser", ""))));

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

            if (string.IsNullOrEmpty(year))
                year = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "span" && n.Attributes.Contains("class") && n.Attributes["class"].Value == "filmCoverSection__year").InnerText;

            var imageUrl = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "img" && n.Attributes.Contains("alt") && n.Attributes.Contains("itemprop") &&
                n.Attributes["itemprop"].Value == "image")?.Attributes["src"].Value;

            var filmInfo = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "div" && n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("filmPosterSection__info filmInfo"))?.Descendants();
            var infoHeaders = filmInfo?.Where(n => n.HasAttributes && n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("filmInfo__header"));

            var filmwebFilmTypes = filmwebHtml.DocumentNode.Descendants()
                .FirstOrDefault(n =>
                    n.Name == "ul" && n.Attributes.Contains("class") &&
                    n.Attributes["class"].Value.Contains("genresList"))?.Descendants().Where(n => n.Name == "a")
                ?.Select(n => n.InnerText);

            var filmwebFilmType = filmwebFilmTypes != null ? string.Join(", ", filmwebFilmTypes) : string.Empty;

            if(string.IsNullOrEmpty(filmwebFilmType))
            {
                var filmTypeHeader = infoHeaders.FirstOrDefault(n => n.InnerText == "gatunek");
                var index = infoHeaders.IndexOf(filmTypeHeader);

                var descendantsTest = filmInfo.Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("filmInfo__info"))
                    .ToList()[index];

                filmwebFilmType = descendantsTest
                    .Descendants()
                    .Where(n => n.Name == "span" && n.InnerText.Trim() != "/")
                    .Select(n=>n.InnerText)
                    .Join(", ");
            }

            var production = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "a" && n.Attributes.Contains("href") && n.Attributes["href"].Value.Contains("countries"))?.InnerText;

            if (string.IsNullOrEmpty(production))
            {
                var prodHeader = infoHeaders.FirstOrDefault(n => n.InnerText == "produkcja");
                var index = infoHeaders.IndexOf(prodHeader);

                production = filmInfo.Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("filmInfo__info")).ToList()[index].Descendants().FirstOrDefault().Descendants().FirstOrDefault().InnerText;
            }

            var filmDesc = filmwebHtml.DocumentNode.Descendants()
                .FirstOrDefault(n =>
                    n.Name == "div" && n.Attributes.Contains("class") &&
                    n.Attributes["class"].Value.Contains("filmPlot"))?
                .Descendants().FirstOrDefault(n => n.Name == "p")?.InnerText;

            if (string.IsNullOrEmpty(filmDesc))
                filmDesc = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n => n.Name == "div" && n.Attributes.Contains("class") && n.Attributes["class"].Value == "filmPosterSection__plot")?.InnerText;

            var cast = filmwebHtml.DocumentNode.Descendants()
                .Where(n => n.Name == "tr"
                        && n.Attributes.Contains("id")
                        && n.Attributes["id"].Value.Contains("role_"))
                            .Select(n => n.Descendants()
                                    .FirstOrDefault(p => p.Name == "td")
                                        .Descendants()
                                            .FirstOrDefault(r => r.Name == "a" && r.Attributes.Contains("title")).Attributes["title"].Value).ToList();

            if(cast.Any() == false)
            {
                cast = filmwebHtml.DocumentNode.Descendants()
                    .Where(n => n.Attributes.Contains("class") && n.Attributes.Contains("data-person"))
                    .Select(n => n.Attributes["data-person"].Value).ToList();
            }

            var ratingValue = 0m;
            decimal.TryParse(rate, out ratingValue);

            var ratingCountValue = 0;
            int.TryParse(rateCount, out ratingCountValue);

            var yearValue = 0;
            int.TryParse(year.OnlyDigits(), out yearValue);

            return new FilmwebResult()
            {
                FilmwebRating = ratingValue,
                FilmwebRatingCount = ratingCountValue,
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

        public List<string> SerializeDirectors(HtmlDocument html)
        {
            var directors = html.DocumentNode.Descendants()
                        .FirstOrDefault(n => n.Name == "li"
                        && n.Attributes.Contains("class")
                        && n.Attributes["class"].Value == "asset-page__meta-list"
                        && n.Descendants().Any(p => p.Name == "span"
                        && p.Attributes.Contains("class")
                        && p.Attributes["class"].Value == "asset-page__meta-label"
                        && p.InnerText == "Reżyseria"))?
                        .InnerText.Replace("\n", "").Replace("Reżyseria", string.Empty)
                        .Split(",").Select(n => n.Trim()).ToList();

            if (directors == null) return new List<string>();

            return directors;
        }

        public string SerializeOriginalTitle(HtmlDocument html)
        {
            var origTitle = html.DocumentNode.Descendants()
                    .FirstOrDefault(n => n.Name == "li"
                    && n.Attributes.Contains("class")
                    && n.Attributes["class"].Value == "asset-page__meta-list"
                    && n.Descendants().Any(p => p.Name == "span"
                    && p.Attributes.Contains("class")
                    && p.Attributes["class"].Value == "asset-page__meta-label"
                    && p.InnerText == "Tytuł oryginalny"))?
                    .InnerText.Replace("\n", "").Replace("Tytuł oryginalny", string.Empty);

            if (string.IsNullOrEmpty(origTitle)) return string.Empty;

            return origTitle
                .Replace(" hd", "")
                .Replace(" HD", "")
                .Replace(" sd", "")
                .Replace(" SD", "");
        }

        public string SerializeFilmwebUrlFromNcPlus(HtmlDocument ncPlusHtml)
        {
            return ncPlusHtml.DocumentNode.Descendants()?
                .Where(n => n.Name == "a" && n.Attributes.Contains("href") && n.Attributes["href"].Value.Contains("filmweb")).FirstOrDefault()
                ?.Attributes["href"].Value;
        }
    }
}
