﻿using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Serialize;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;

namespace vod.Domain.Services
{
    public class FilmwebService : IFilmwebService
    {
        private readonly IHtmlSourceGetter _sourceGetter;
        private readonly IHtmlSourceSerializer _sourceSerializer;
        private readonly IVodRepositoryBackground _repositoryBackground;
        private readonly IMapper _mapper;
        private IList<ResultModel> _storedData;

        public FilmwebService(
            IHtmlSourceGetter sourceGetter,
            IHtmlSourceSerializer sourceSerializer,
            IVodRepositoryBackground repositoryBackground,
            IMapper mapper)
        {
            _sourceGetter = sourceGetter;
            _sourceSerializer = sourceSerializer;
            _repositoryBackground = repositoryBackground;
            _mapper = mapper;
        }

        public FilmwebResult GetFilmwebResult(Movie movie)
        {
            if (_storedData == null || !_storedData.Any())
                _storedData = _repositoryBackground.GetResultsOfType((int)movie.MovieType);

            return GetFromStoredData(movie) ?? SearchFilmwebResult(movie);
        }

        private FilmwebResult SearchFilmwebResult(Movie movie)
        {
            var moreInfoHtml = _sourceGetter.GetHtmlFrom($"{NcPlusUrls.NcPlusGoUrl}{movie.MoreInfoUrl}");
            movie.OriginalTitle = _sourceSerializer.SerializeOriginalTitle(moreInfoHtml);
            movie.Directors = _sourceSerializer.SerializeDirectors(moreInfoHtml);

            var filmwebSearchHtml = _sourceGetter.GetHtmlFrom(FilmwebUrls.FilmwebSearchBaseUrl(movie.OriginalTitle));
            var filmwebUrl = _sourceSerializer.SerializeFilmwebUrl(filmwebSearchHtml, movie.Directors);

            if (string.IsNullOrEmpty(filmwebUrl))
                return null;

            var filmwebHtml = _sourceGetter.GetHtmlFrom(filmwebUrl);
            var result = _sourceSerializer.SerializeFilmwebResult(filmwebHtml, movie.MovieType, movie.MoreInfoUrl, movie.Title);
            result.ProviderName = movie.ProviderName;

            return result;
        }

        private FilmwebResult GetFromStoredData(Movie movie)
        {
            if (_storedData.Any(n => n.Title == movie.Title))
                return _mapper.Map<FilmwebResult>(_storedData.FirstOrDefault(n => n.Title == movie.Title));

            return null;
        }
    }
}
