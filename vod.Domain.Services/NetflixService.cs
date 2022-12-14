using Microsoft.Extensions.Configuration;
using RestSharp;
using System.Collections.Generic;
using System.Text;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Serialize;

namespace vod.Domain.Services
{
    public class NetflixService : INetflixService
    {
        private readonly IHtmlSourceSerializer _htmlSourceSerializer;
        private readonly IUrlGetter _urlGetter;
        private readonly IConfiguration _configuration;

        public NetflixService(
            IHtmlSourceSerializer htmlSourceSerializer, 
            IUrlGetter urlGetter,
            IConfiguration configuration)
        {
            _htmlSourceSerializer = htmlSourceSerializer;
            _urlGetter = urlGetter;
            _configuration = configuration;
        }

        public IEnumerable<NetflixResult> GetMoviesOfType(MovieTypes type)
        {
            var apiUrl = _urlGetter.GetNetflixApiUrl(type);
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);

            request.AddHeader("x-rapidapi-host", "unogs-unogs-v1.p.rapidapi.com");

            var apikey = _configuration.GetValue<string>("netflix_key");
            request.AddHeader("x-rapidapi-key", apikey);

            IRestResponse response = client.Execute(request);            
            
            var result = _htmlSourceSerializer.SerializeMoviesNetflix(response.Content, type);
            return result;
        }
    }
}
