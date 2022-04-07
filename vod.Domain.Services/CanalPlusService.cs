using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource.Serialize;

namespace vod.Domain.Services
{
    public class CanalPlusService : ICanalPlusService
    {
        private IHtmlSourceSerializer _htmlSourceSerializer;

        public CanalPlusService(IHtmlSourceSerializer htmlSourceSerializer)
        {
            _htmlSourceSerializer = htmlSourceSerializer;
        }

        public IEnumerable<CanalPlusResult> GetMoviesOfType(MovieTypes type)
        {
            var apiUrl = CanalPlusUrls.GetUrlWithType(type);
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);

            var response = client.Execute(request);

            var result = _htmlSourceSerializer.SerializeCanalPlusMovies(response.Content, type);
            return result;
        }
    }
}
