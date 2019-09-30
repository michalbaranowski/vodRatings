using System.Collections.Generic;
using HtmlAgilityPack;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Utils.HtmlSource.Deserialize
{
    public interface IHtmlSourceDeserializer
    {
        IEnumerable<Movie> DeserializeMovies(HtmlDocument html);
        string DeserializeFilmwebUrl(HtmlDocument html);
        FilmwebResult DeserializeFilmwebResult(HtmlDocument filmwebHtml);
    }
}