using System.Collections.Generic;
using System.Linq;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Utils
{
    public class UrlGetter : IUrlGetter
    {
        public IEnumerable<string> GetNcPlusBaseUrls()
        {
            var props = typeof(NcPlusUrls).GetFields().Where(n => n.Name.ToLower().Contains("base"));
            return props.Select(n => n.GetValue(null).ToString());
        }

        public string GetNetflixUrl(MovieTypes type)
        {
            var baseUrl = NetflixUrls.NetflixBaseUrl;
            var fullUrl = NetflixUrls.GetUrlWithType(baseUrl, type);
            return fullUrl;
        }
    }

    public interface IUrlGetter
    {
        IEnumerable<string> GetNcPlusBaseUrls();
        string GetNetflixUrl(MovieTypes type);
    }
}
