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
                    return $"{baseUrl}/browse/genre/8933?so=yr";
                case MovieTypes.Action:
                    return $"{baseUrl}/browse/genre/1365?so=yr";
                case MovieTypes.Comedy:
                    return $"{baseUrl}/browse/genre/6548?so=yr";
                case MovieTypes.Cartoons:
                    return $"{baseUrl}/browse/genre/6218?so=yr";
                default:
                    throw new NotImplementedException("Brak implementacji dla podanego typu!");
            }
        }
    }
}
