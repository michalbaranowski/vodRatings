using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Utils;

namespace vod.Domain.Services
{
    public class DisneyPlusService : IDisneyPlusService
    {
        private readonly IDisneyPlusSerializer _serializer;

        public DisneyPlusService(IDisneyPlusSerializer serializer)
        {
            _serializer = serializer;
        }

        public IEnumerable<FilmResultWithMovieType> GetMoviesOfType(MovieTypes type)
        {
            var htmlContent = GetHtmlContent(DisneyPlusUrls.DisneyPlusBaseUrl);
            var urls = _serializer.SerializeUrls(htmlContent).Where(n => n.StartsWith(DisneyPlusUrls.HttpBegining));
            var urlsOfMovies = urls.Where(n => n.Contains(DisneyPlusUrls.MoviesTagInUrl));
            var urlsToBeSerializedAgain = urls.Where(n => n.Contains(DisneyPlusUrls.MoviesTagInUrl) == false);

            var additionalMoviesUrls = urlsToBeSerializedAgain.SelectMany(url =>
            {
                var currentHtmlContent = GetHtmlContent(url);
                var urls2 = _serializer.SerializeUrls(currentHtmlContent);
                return urls2;
            });

            var allMoviesUrls = urlsOfMovies.Concat(additionalMoviesUrls).Where(n => n.StartsWith(DisneyPlusUrls.HttpBegining)).ToList();
            var results = allMoviesUrls.Select(url =>
            {
                var currentHtmlContent = GetHtmlContent(url);
                return _serializer.Serialize(currentHtmlContent);
            }).ToList();

            return results;
        }

        private string GetHtmlContent(string url)
        {
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            var response = client.Execute(request);
            return response.Content;
        }
    }
}
