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
        private readonly IHtmlSourceGetter _htmlSourceGetter;
        private readonly IHtmlSourceSerializer _htmlSourceSerializer;
        private readonly IUrlGetter _urlGetter;

        public NetflixService(
            IHtmlSourceGetter htmlSourceGetter, 
            IHtmlSourceSerializer htmlSourceSerializer, 
            IUrlGetter urlGetter)
        {
            _htmlSourceGetter = htmlSourceGetter;
            _htmlSourceSerializer = htmlSourceSerializer;
            _urlGetter = urlGetter;
        }

        public IEnumerable<NetflixResult> GetMoviesOfType(MovieTypes type)
        {
            var apiUrl = _urlGetter.GetNetflixApiUrl(type);
            var client = new RestClient(apiUrl);
            var request = new RestRequest(Method.GET);
            request.AddHeader("x-rapidapi-host", "unogs-unogs-v1.p.rapidapi.com");
            request.AddHeader("x-rapidapi-key", "f4dfcd2171msh5845838bd3bcdecp130b03jsncf5dbca23307");
            IRestResponse response = client.Execute(request);            
            
            var result = _htmlSourceSerializer.SerializeMoviesNetflix(response.Content, type);
            return result;
        }
    }
}
