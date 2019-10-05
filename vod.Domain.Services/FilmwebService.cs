using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Deserialize;

namespace vod.Domain.Services
{
    public class FilmwebService : IFilmwebService, IFilmwebUrlGetter
    {
        private readonly IHtmlSourceGetter _sourceGetter;
        private readonly IHtmlSourceDeserializer _sourceDeserializer;

        public FilmwebService(
            IHtmlSourceGetter sourceGetter,
            IHtmlSourceDeserializer sourceDeserializer)
        {
            _sourceGetter = sourceGetter;
            _sourceDeserializer = sourceDeserializer;
        }

        public FilmwebResult CheckInFilmweb(Movie movie)
        {
            var filmwebUrl = GetFilmwebUrl(movie);

            if (string.IsNullOrEmpty(filmwebUrl))
                return null;

            var filmwebHtml = _sourceGetter.GetHtmlFrom(filmwebUrl).Result;
            var result = _sourceDeserializer.DeserializeFilmwebResult(filmwebHtml, movie.MovieType);
            result.ProviderName = movie.ProviderName;
            return result;
        }

        public string GetFilmwebUrl(Movie movie)
        {
            var moreInfoHtml = _sourceGetter.GetHtmlFrom($"{NcPlusUrls.NcPlusBaseUrl}{movie.MoreInfoUrl}").Result;
            var filmwebUrl = _sourceDeserializer.DeserializeFilmwebUrl(moreInfoHtml);
            return filmwebUrl;
        }
    }
}
