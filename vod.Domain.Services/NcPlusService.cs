using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Serialize;

namespace vod.Domain.Services
{
    public class NcPlusService : INcPlusService
    {
        private readonly IHtmlSourceGetter _sourceGetter;
        private readonly IHtmlSourceSerializer _serializer;
        private readonly IUrlGetter _urlGetter;
        private IEnumerable<Movie> _results;
        private DateTime _refreshDate;

        public NcPlusService(
            IHtmlSourceGetter sourceGetter,
            IHtmlSourceSerializer serializer,
            IUrlGetter urlGetter)
        {
            _sourceGetter = sourceGetter;
            _serializer = serializer;
            _urlGetter = urlGetter;
        }

        public IEnumerable<Movie> GetMoviesOfType(MovieTypes type)
        {
            if (_results != null && _results.Any() && _refreshDate.AddHours(1) >= DateTime.Now)
                return _results;

            var urls = _urlGetter.GetBaseUrls();
            var result = new List<Movie>().AsEnumerable();

            foreach (var baseUrl in urls)
            {
                var html = _sourceGetter.GetHtmlFrom(baseUrl.GetUrlWithType(type));
                var serialized = _serializer.SerializeMovies(html, type);
                result = result.Concat(serialized);
            }

            _results = result.DistinctBy(n => n.Title);
            _refreshDate = DateTime.Now;
            return _results;
        }
    }
}
