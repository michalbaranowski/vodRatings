﻿using HtmlAgilityPack;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Enums;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
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

        public async Task<IEnumerable<Movie>> GetMoviesOfType(MovieTypes type)
        {
            var cplusHtml = await _sourceGetter.GetHtmlFrom(NcPlusUrls.VodCplusBaseUrl);
            var cplusResult = _deserializer.DeserializeMovies(cplusHtml);

            var hboHtml = await _sourceGetter.GetHtmlFrom(NcPlusUrls.VodHboBaseUrl);
            var hboResult = _deserializer.DeserializeMovies(hboHtml);

            var result = cplusResult.Concat(hboResult).DistinctBy(n=>n.Title);
            return result;
        }
    }
}
