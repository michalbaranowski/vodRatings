using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using NodaTime;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils.HtmlSource.Extension;
using vod.Domain.Services.Utils.HtmlSource.Model;
using vod.Domain.Services.Utils.HtmlSource.Model.CanalPlus;

namespace vod.Domain.Services.Utils.HtmlSource.Serialize
{
    public class HtmlSourceSerializer : IHtmlSourceSerializer
    {
        public IEnumerable<NcPlusResult> SerializeMoviesNcPlus(HtmlDocument html, MovieTypes type)
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

        public IEnumerable<NetflixResult> SerializeMoviesNetflix(string json, MovieTypes type)
        {
            //latanie po jsonie
            NetflixJsonResult deserialized = JsonConvert.DeserializeObject<NetflixJsonResult>(json);
            var results = deserialized.Items.Select(n => new NetflixResult()
            {
                Title = n.Title,
                MovieType = type,
                ProviderName = "Netflix",
                Url = NetflixUrls.NetflixBaseUrl + "/watch/" + n.NetflixId
            });

            return results;
        }

        public string SerializeFilmwebUrl(HtmlDocument html, List<string> directors)
        {
            var url = html.DocumentNode.Descendants("a")
                .FirstOrDefault(n => n.Attributes.Any(attr => attr.Name == "class") && n.Attributes["class"].Value == "preview__link")?
                .Attributes["href"].Value;

            if (string.IsNullOrEmpty(url))
            {
                return string.Empty;
            }

            if (url.Contains(FilmwebUrls.FilmwebBaseUrl) == false)
            {
                url = FilmwebUrls.FilmwebBaseUrl + url;
            }

            return url;
        }

        public FilmwebResult SerializeFilmwebResult(HtmlDocument filmwebHtml, MovieTypes movieType, string movieUrl, string movieTitle)
        {
            try
            {
                return SerializeFilmwebResultInternal(filmwebHtml, movieType, movieUrl, movieTitle);
            }
            catch (Exception exp)
            {
                //TODO: check why sometimes exception throws here
                return new FilmwebResult();
            }
        }

        private FilmwebResult SerializeFilmwebResultInternal(HtmlDocument filmwebHtml, MovieTypes movieType, string movieUrl, string movieTitle)
        {
            var rate = filmwebHtml.DocumentNode.Descendants()?.FirstOrDefault(n => n.Name == "span" && n.Attributes.Contains("itemprop") &&
                n.Attributes["itemprop"].Value.Contains("ratingValue"))?.InnerHtml;

            var rateCount = filmwebHtml.DocumentNode.Descendants()?
                .FirstOrDefault(n => n.Name == "span" && n.Attributes.Contains("itemprop") &&
                                     n.Attributes["itemprop"].Value.Contains("ratingCount"))?.InnerHtml;

            var title = filmwebHtml.DocumentNode.Descendants("div")
                .FirstOrDefault(n => n.Attributes.Contains("class") && n.Attributes["class"].Value == "filmCoverSection__titleDetails")
                .Descendants("h1").FirstOrDefault()?.InnerText;

            var year = filmwebHtml.DocumentNode.Descendants("div")
                .FirstOrDefault(n => n.Attributes.Contains("class") &&
                                n.Attributes["class"].Value == "filmCoverSection__year")?
                .InnerText;

            var durationInMinutesStr = filmwebHtml.DocumentNode.Descendants("div")
                .FirstOrDefault(n => n.Attributes.Contains("class") &&
                                n.Attributes["class"].Value == "filmCoverSection__duration")?
                                .Attributes.FirstOrDefault(n => n.Name == "data-duration").Value;

            var imageUrl = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "img" && n.Attributes.Contains("alt") && n.Attributes.Contains("itemprop") &&
                n.Attributes["itemprop"].Value == "image")?.Attributes["src"].Value;

            var filmInfo = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "div" && n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("filmPosterSection__info filmInfo"))?.Descendants();
            var infoHeaders = filmInfo?.Where(n => n.HasAttributes && n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("filmInfo__header"));

            var filmwebFilmTypes = filmwebHtml.DocumentNode.Descendants()
                .FirstOrDefault(n =>
                    n.Name == "ul" && n.Attributes.Contains("class") &&
                    n.Attributes["class"].Value.Contains("genresList"))?.Descendants()
                    .Where(n => n.Name == "a")
                ?.Select(n => n.InnerText.Trim());

            var filmwebFilmType = filmwebFilmTypes != null ? string.Join(", ", filmwebFilmTypes) : string.Empty;

            if (string.IsNullOrEmpty(filmwebFilmType))
            {
                var filmTypeHeader = infoHeaders.FirstOrDefault(n => n.InnerText == "gatunek");
                var index = infoHeaders.IndexOf(filmTypeHeader);

                var descendantsTest = filmInfo.Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("filmInfo__info"))
                    .ToList()[index];

                filmwebFilmType = descendantsTest
                    .Descendants()
                    .Where(n => n.Name == "span" && n.Descendants().Any(p => p.Name == "a") && n.InnerText.Trim() != "/")
                    .Select(n => n.InnerText.Trim())
                    .Join(", ");
            }

            var production = filmwebHtml.DocumentNode.Descendants().FirstOrDefault(n =>
                n.Name == "a" && n.Attributes.Contains("href") && n.Attributes["href"].Value.Contains("countries"))?.InnerText;

            if (string.IsNullOrWhiteSpace(production))
            {
                var prodHeader = infoHeaders.FirstOrDefault(n => n.InnerText == "produkcja");
                var index = infoHeaders.IndexOf(prodHeader);

                production = filmInfo.Where(n => n.Attributes.Contains("class") && n.Attributes["class"].Value.Contains("filmInfo__info")).ToList()[index].Descendants().FirstOrDefault().Descendants().FirstOrDefault().InnerText;
            }

            if (string.IsNullOrWhiteSpace(production))
            {
                production = filmwebHtml.DocumentNode.Descendants("div")
                    .FirstOrDefault(n => n.Attributes.Any(attr => attr.Name == "class") &&
                                                n.Attributes["class"].Value == "filmInfo__info" &&
                                                n.Descendants("span").Count() == 1 && n.Descendants("span").First()
                                                                                .Descendants("a").Any(atag => atag.Attributes["href"].Value.Contains("ranking")))?
                    .InnerText.Trim() ?? string.Empty;
            }

            var filmDesc = filmwebHtml.DocumentNode.Descendants()
            .FirstOrDefault(n =>
                n.Name == "span" && n.Attributes.Contains("itemprop") &&
                n.Attributes["itemprop"].Value.Contains("description"))?.InnerText;

            var cast = filmwebHtml.DocumentNode.Descendants("h3")
                .Where(n => n.Attributes.Any(attr => attr.Name == "class") && n.Attributes["class"].Value == "simplePoster__heading")
                .Select(n => n.InnerText);

            var ratingValue = 0m;
            decimal.TryParse(rate, out ratingValue);

            var ratingCountValue = 0;
            int.TryParse(rateCount, out ratingCountValue);

            var yearValue = 0;
            int.TryParse(year.OnlyDigits(), out yearValue);

            var durationInMinutes = 0;
            int.TryParse(durationInMinutesStr, out durationInMinutes);

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
                MovieUrl = movieUrl,
                Cast = cast.Distinct().ToList(),
                Duration = Duration.FromMinutes(durationInMinutes)
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

        public IEnumerable<CanalPlusResult> SerializeCanalPlusMovies(string content, MovieTypes type)
        {
            CanalPlusApiResponse deserialized = JsonConvert.DeserializeObject<CanalPlusApiResponse>(content);

            if (deserialized.Contents == null)
            {
                return Enumerable.Empty<CanalPlusResult>();
            }

            var results = new List<CanalPlusResult>();

            foreach (var contentItem in deserialized.Contents)
            {
                var movieType = GetMovieTypeBySubTitle(contentItem.Subtitle);

                if (movieType == null) continue;

                var result = new CanalPlusResult()
                {
                    Title = contentItem.Title,
                    MovieType = movieType.Value,
                    ProviderName = "CanalPlus",
                    Url = $"{CanalPlusUrls.CanalPlusBaseUrl}{contentItem.OnClick.Path}"
                };

                results.Add(result);
            }

            return results;
        }

        private MovieTypes? GetMovieTypeBySubTitle(string subtitle)
        {
            if (subtitle.Contains("Komedia"))
            {
                return MovieTypes.Comedy;
            }

            else if (subtitle.Contains("Thriller") || subtitle.Contains("Kryminał"))
            {
                return MovieTypes.Thriller;
            }

            else if (subtitle.Contains("Akcja"))
            {
                return MovieTypes.Action;
            }

            else if (subtitle.Contains("Familijny") ||
                     subtitle.Contains("Animowany") ||
                     subtitle.Contains("Dla dzieci") ||
                     subtitle.Contains("Przygodowy"))
            {
                return MovieTypes.Cartoons;
            }

            return null;
        }
    }
}
