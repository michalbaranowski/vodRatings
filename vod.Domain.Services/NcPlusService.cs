﻿using MoreLinq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Deserialize;

namespace vod.Domain.Services
{
    public class NcPlusService : INcPlusService
    {
        private readonly IHtmlSourceGetter _sourceGetter;
        private readonly IHtmlSourceDeserializer _deserializer;

        public NcPlusService(
            IHtmlSourceGetter sourceGetter,
            IHtmlSourceDeserializer deserializer)
        {
            _sourceGetter = sourceGetter;
            _deserializer = deserializer;
        }

        public IEnumerable<Movie> GetMoviesOfType(MovieTypes type)
        {
            var cplusHtml = _sourceGetter.GetHtmlFrom(NcPlusUrls.VodCplusBaseUrl.GetUrlWithType(type));
            var cplusResult = _deserializer.DeserializeMovies(cplusHtml, type);

            var premieryHtml = _sourceGetter.GetHtmlFrom(NcPlusUrls.VodPremieryBaseUrl.GetUrlWithType(type));
            var premieryResult = _deserializer.DeserializeMovies(premieryHtml, type);

            var hboHtml = _sourceGetter.GetHtmlFrom(NcPlusUrls.VodHboBaseUrl.GetUrlWithType(type));
            var hboResult = _deserializer.DeserializeMovies(hboHtml, type);

            var result = cplusResult
                .Concat(hboResult)
                .Concat(premieryResult)
                .DistinctBy(n=>n.Title);

            return result;
        }
    }
}
