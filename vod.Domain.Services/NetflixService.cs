using System.Collections.Generic;
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
            var fullUrl = _urlGetter.GetNetflixUrl(type);
            var html = _htmlSourceGetter.GetHtmlFrom(fullUrl);
            var result = _htmlSourceSerializer.SerializeMoviesNetflix(html, type);
            return result;
        }
    }
}
