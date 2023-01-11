using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Utils
{
    public class UrlGetter : IUrlGetter
    {

        public string GetNetflixApiUrl(MovieTypes type)
        {
            var baseUrl = "https://unogs-unogs-v1.p.rapidapi.com/aaapi.cgi?q=%7Bquery%7D-!1900%2C2020-!0%2C5-!0%2C10-!{0}-!Any-!Polish-!Polish-!gt1000-!%7Bdownloadable%7D&t=ns&cl=all&st=adv&ob=Rating&p=1&sa=or";
            var fullUrl = NetflixUrls.GetUrlWithType(baseUrl, type);
            return fullUrl;
        }
    }

    public interface IUrlGetter
    {
        string GetNetflixApiUrl(MovieTypes type);
    }
}
