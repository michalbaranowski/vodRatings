using System.Collections.Generic;
using HtmlAgilityPack;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Utils.HtmlSource.Serialize
{
    public interface IHtmlSourceSerializer
    {
        IEnumerable<Movie> SerializeMovies(HtmlDocument html, MovieTypes type);
        FilmwebResult SerializeFilmwebResult(HtmlDocument filmwebHtml, MovieTypes movieMovieType, string movieName);
        string SerializeFilmwebUrl(HtmlDocument filmwebSearchHtml, List<string> directors);
    }
}