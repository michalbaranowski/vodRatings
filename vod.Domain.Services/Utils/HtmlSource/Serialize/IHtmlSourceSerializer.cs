using System.Collections.Generic;
using HtmlAgilityPack;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Utils.HtmlSource.Serialize
{
    public interface IHtmlSourceSerializer
    {
        IEnumerable<NcPlusResult> SerializeMoviesNcPlus(HtmlDocument html, MovieTypes type);
        IEnumerable<NetflixResult> SerializeMoviesNetflix(string json, MovieTypes type);
        FilmwebResult SerializeFilmwebResult(HtmlDocument filmwebHtml, MovieTypes movieType, string moreInfoUrl, string movieTitle);
        string SerializeFilmwebUrl(HtmlDocument filmwebSearchHtml, List<string> directors);
        string SerializeFilmwebUrlFromNcPlus(HtmlDocument ncPlusHtml);
        List<string> SerializeDirectors(HtmlDocument html);
        string SerializeOriginalTitle(HtmlDocument html);
        IEnumerable<CanalPlusResult> SerializeCanalPlusMovies(string content, MovieTypes type);
    }
}