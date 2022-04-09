using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using NUnit.Framework;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Tests.Resources;
using vod.Domain.Services.Utils.HtmlSource.Serialize;

namespace vod.Domain.Services.Tests
{
    public class HtmlSourceSerializerUT
    {
        private HtmlSourceSerializer _serializer;
        private HtmlDocument _moviesHtmlDoc;
        private HtmlDocument _moreInfoHtmlDoc;
        private HtmlDocument _filmwebResultHtmlDoc;
        private HtmlDocument _cplusComedies;
        private HtmlDocument _ncPremieresComediesDoc;
        private HtmlDocument _hboComedies;
        private HtmlDocument _filmwebSearch2HtmlDoc;

        private void Arrange()
        {
            _serializer = new HtmlSourceSerializer();
            
            var ncPremieresComedies = HtmlResources.NcPremieresResultHtml();
            _ncPremieresComediesDoc = new HtmlDocument();
            _ncPremieresComediesDoc.LoadHtml(ncPremieresComedies);

            var hboComedies = HtmlResources.HboComediesResultHtml();
            _hboComedies = new HtmlDocument();
            _hboComedies.LoadHtml(hboComedies);

            var moviesHtml = HtmlResources.CanalPlusThrillersResultHtml();
            _moviesHtmlDoc = new HtmlDocument();
            _moviesHtmlDoc.LoadHtml(moviesHtml);

            var cplusComedies = HtmlResources.CanalPlusComediesResultHtml();
            _cplusComedies = new HtmlDocument();
            _cplusComedies.LoadHtml(cplusComedies);

            var moreInfoHtml = HtmlResources.CanalPlusConcrteMovieResultHtml();
            _moreInfoHtmlDoc = new HtmlDocument();
            _moreInfoHtmlDoc.LoadHtml(moreInfoHtml);

            var filmwebResultHtml = HtmlResources.FilmwebResultHtml();
            _filmwebResultHtmlDoc = new HtmlDocument();
            _filmwebResultHtmlDoc.LoadHtml(filmwebResultHtml);

            var filmwebSearch2Html = HtmlResources.FilmWebSearchResult2Html();
            _filmwebSearch2HtmlDoc = new HtmlDocument();
            _filmwebSearch2HtmlDoc.LoadHtml(filmwebSearch2Html);
        }

        [Test]
        public void SerializeMovies_ShouldSerializeCorrectValues()
        {
            Arrange();

            var result = _serializer.SerializeMoviesNcPlus(_moviesHtmlDoc, MovieTypes.Thriller);

            Assert.True(result.Any(n=>n.Title == "Ostateczna rozgrywka"));
            Assert.True(result.Any(n => n.MoreInfoUrl == "/Collection/Asset?codename=ostateczna-rozgrywka-19"));
            Assert.True(result.Any(n => n.MovieType == MovieTypes.Thriller));
        }

        [Test]
        public void SerializeMovies_ShouldSerializeCorrectProviderName()
        {
            Arrange();

            var result1 = _serializer.SerializeMoviesNcPlus(_cplusComedies, MovieTypes.Comedy);
            var result2 = _serializer.SerializeMoviesNcPlus(_hboComedies, MovieTypes.Comedy);
            var result3 = _serializer.SerializeMoviesNcPlus(_ncPremieresComediesDoc, MovieTypes.Comedy);

            Assert.True(result1.All(n => n.ProviderName == "CANAL+ VOD"));
            Assert.True(result2.All(n => n.ProviderName.ToLower().Contains("hbo")));
            Assert.True(result3.All(n => n.ProviderName.ToLower().Contains("premiery")));
        }

        [Test]
        public void SerializeFilmwebUrl_ShouldSerializeCorrectValues()
        {
            Arrange();
            var directors = new List<string>() { "David Mackenzie", "Test" };

            var result = _serializer.SerializeFilmwebUrl(_filmwebSearch2HtmlDoc, directors);

            Assert.AreEqual("https://www.filmweb.pl/film/A%C5%BC+do+piek%C5%82a-2016-741821", result);
        }

        [Test]
        public void SerializeFilmwebResult_ShouldSerializeCorrectValues()
        {
            Arrange();

            var title = "Aż do piekła";
            var result = _serializer.SerializeFilmwebResult(_filmwebResultHtmlDoc, MovieTypes.Thriller, string.Empty, title);

            Assert.True(result.Title == title);
            Assert.True(result.FilmwebRating == 7.1m);
            Assert.True(result.Production == "USA");
            Assert.True(result.FilmwebFilmType == "Dramat, Kryminał");
            Assert.True(result.FilmDescription.Contains("Rozwiedziony ojciec i jego brat"));
            Assert.True(result.Year == 2016);
            Assert.True(result.Cast.Contains("Jeff Bridges"));
        }
    }
}
