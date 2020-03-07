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

        public string GetNetflixApiUrl(MovieTypes type)
        {
            var baseUrl = "https://unogs-unogs-v1.p.rapidapi.com/aaapi.cgi?q=%7Bquery%7D-!1900%2C2020-!0%2C5-!0%2C10-!{0}-!Any-!Polish-!Polish-!gt100-!%7Bdownloadable%7D&t=ns&cl=all&st=adv&ob=Rating&p=1&sa=or";
            var fullUrl = NetflixUrls.GetUrlWithType(baseUrl, type);
            return fullUrl;
        }
    }

    public interface IUrlGetter
    {
        IEnumerable<string> GetNcPlusBaseUrls();
        string GetNetflixApiUrl(MovieTypes type);
    }
}
