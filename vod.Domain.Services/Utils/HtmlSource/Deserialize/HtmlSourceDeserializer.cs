using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using vod.Core.Boundary.Model;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils.HtmlSource.Extension;

namespace vod.Domain.Services.Utils.HtmlSource.Deserialize
{
    public class HtmlSourceDeserializer : IHtmlSourceDeserializer
    {
        public IEnumerable<Movie> DeserializeMovies(HtmlDocument html)
        {
            var divs = html.DocumentNode.Descendants("div")
                .Where(x => x.Attributes.Contains("title"));

            return divs.Select(n =>
                    new Movie()
                    {
                        Title = n.Attributes?
                            .FirstOrDefault(p => p.Name == "title")?
                            .Value
                            .Replace("(HD)", string.Empty)
                            .Replace("(SD)", ""),
                        
                        MoreInfoUrl = n.Descendants().Where(x=>x.Name == "a" && x.Attributes.Contains("href"))?.FirstOrDefault()?.Attributes["href"].Value
                    });
        }

        public string DeserializeFilmwebUrl(HtmlDocument html)
        {
            return html.DocumentNode.Descendants()?
                .Where(n => n.Name == "a" && n.Attributes.Contains("href") && n.Attributes["href"].Value.Contains("filmweb")).FirstOrDefault()
                ?.Attributes["href"].Value;
        }

        public Result DeserializeFilmwebResult(HtmlDocument filmwebHtml)
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

            return new Result() {FilmwebRating = Convert.ToDecimal(rate), FilmwebRatingCount  = Convert.ToInt32(rateCount), Title = title, Year = Convert.ToInt32(year)};
        }
    }
}
