using System;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Utils
{
    public static class NetflixUrls
    {
        public static readonly string NetflixBaseUrl = "http://netflix.com";

        public static string GetUrlWithType(string baseUrl, MovieTypes type)
        {
            switch (type)
            {
                case MovieTypes.Thriller:
                    return string.Format(baseUrl, 8933);
                case MovieTypes.Action:
                    return string.Format(baseUrl, 1365);
                case MovieTypes.Comedy:
                    return string.Format(baseUrl, 6548);
                case MovieTypes.Cartoons:
                    return string.Format(baseUrl, 6218);
                default:
                    throw new NotImplementedException("Brak implementacji dla podanego typu!");
            }
        }
    }
}
