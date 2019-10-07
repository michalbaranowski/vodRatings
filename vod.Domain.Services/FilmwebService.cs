using AutoMapper;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Deserialize;
using vod.Repository.Boundary;

namespace vod.Domain.Services
{
    public class FilmwebService : IFilmwebService, IFilmwebUrlGetter
    {
        private readonly IHtmlSourceGetter _sourceGetter;
        private readonly IHtmlSourceDeserializer _sourceDeserializer;
        private readonly IVodRepositoryBackground _repositoryBackground;
        private readonly IMapper _mapper;

        public FilmwebService(
            IHtmlSourceGetter sourceGetter,
            IHtmlSourceDeserializer sourceDeserializer,
            IVodRepositoryBackground repositoryBackground,
            IMapper mapper)
        {
            _sourceGetter = sourceGetter;
            _sourceDeserializer = sourceDeserializer;
            _repositoryBackground = repositoryBackground;
            _mapper = mapper;
        }

        public FilmwebResult CheckInFilmweb(Movie movie)
        {
            var storedData = _repositoryBackground.ResultByTitle(movie.Title);
            if (storedData != null)
                return _mapper.Map<FilmwebResult>(storedData);

            var filmwebUrl = GetFilmwebUrl(movie);

            if (string.IsNullOrEmpty(filmwebUrl))
                return null;

            var filmwebHtml = _sourceGetter.GetHtmlFrom(filmwebUrl);
            var result = _sourceDeserializer.DeserializeFilmwebResult(filmwebHtml, movie.MovieType);
            result.ProviderName = movie.ProviderName;
            return result;
        }

        public string GetFilmwebUrl(Movie movie)
        {
            var moreInfoHtml = _sourceGetter.GetHtmlFrom($"{NcPlusUrls.NcPlusBaseUrl}{movie.MoreInfoUrl}");
            var filmwebUrl = _sourceDeserializer.DeserializeFilmwebUrl(moreInfoHtml);
            return filmwebUrl;
        }
    }
}
