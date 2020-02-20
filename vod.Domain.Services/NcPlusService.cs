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
        private Dictionary<MovieTypes, IEnumerable<NcPlusResult>> _results;
        private Dictionary<MovieTypes, DateTime> _refreshDate;

        public NcPlusService(
            IHtmlSourceGetter sourceGetter,
            IHtmlSourceSerializer serializer,
            IUrlGetter urlGetter)
        {
            _sourceGetter = sourceGetter;
            _serializer = serializer;
            _urlGetter = urlGetter;

            _results = new Dictionary<MovieTypes, IEnumerable<NcPlusResult>>();
            _refreshDate = new Dictionary<MovieTypes, DateTime>();
        }

        public IEnumerable<NcPlusResult> GetMoviesOfType(MovieTypes type)
        {
            if (IsStoredDataAvailable(type))
                return _results[type];

            var urls = _urlGetter.GetBaseUrls();
            var result = new List<NcPlusResult>().AsEnumerable();

            foreach (var baseUrl in urls)
            {
                var html = _sourceGetter.GetHtmlFrom(baseUrl.GetUrlWithType(type));
                var serialized = _serializer.SerializeMovies(html, type);
                result = result.Concat(serialized);
            }

            StoreData(type, result);
            return _results[type];
        }

        private bool IsStoredDataAvailable(MovieTypes type)
        {
            return _results.ContainsKey(type) && _results[type] != null && _results[type].Any() && _refreshDate[type].AddHours(1) >= DateTime.Now;
        }

        private void StoreData(MovieTypes type, IEnumerable<NcPlusResult> result)
        {
            if (_results.ContainsKey(type) == false)
                _results.Add(type, result.DistinctBy(n => n.Title));
            else
                _results[type] = result.DistinctBy(n => n.Title);

            if (_refreshDate.ContainsKey(type) == false)
                _refreshDate.Add(type, DateTime.Now);
            else
                _refreshDate[type] = DateTime.Now;
        }
    }
}
