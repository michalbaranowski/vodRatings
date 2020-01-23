using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private HtmlDocument _filmwebSearchHtmlDoc;
        private HtmlDocument _filmwebSearch2HtmlDoc;
        private HtmlDocument _filmwebNewResultHtmlDoc;

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

            var filmwebSearchHtml = HtmlResources.FilmWebSearchResultHtml();
            _filmwebSearchHtmlDoc = new HtmlDocument();
            _filmwebSearchHtmlDoc.LoadHtml(filmwebSearchHtml);

            var filmwebResultHtml = HtmlResources.FilmwebResultHtml();
            _filmwebResultHtmlDoc = new HtmlDocument();
            _filmwebResultHtmlDoc.LoadHtml(filmwebResultHtml);

            var filmwebNewResultHtml = HtmlResources.FilmwebNewResultHtml();
            _filmwebNewResultHtmlDoc = new HtmlDocument();
            _filmwebNewResultHtmlDoc.LoadHtml(filmwebNewResultHtml);

            var filmwebSearch2Html = HtmlResources.FilmWebSearchResult2Html();
            _filmwebSearch2HtmlDoc = new HtmlDocument();
            _filmwebSearch2HtmlDoc.LoadHtml(filmwebSearch2Html);
        }

        [Test]
        public void SerializeMovies_ShouldSerializeCorrectValues()
        {
            Arrange();

            var result = _serializer.SerializeMovies(_moviesHtmlDoc, MovieTypes.Thriller);

            Assert.True(result.Any(n=>n.Title == "Ostateczna rozgrywka"));
            Assert.True(result.Any(n => n.MoreInfoUrl == "/Collection/Asset?codename=ostateczna-rozgrywka-19"));
            Assert.True(result.Any(n => n.MovieType == MovieTypes.Thriller));
        }

        [Test]
        public void SerializeMovies_ShouldSerializeCorrectProviderName()
        {
            Arrange();

            var result1 = _serializer.SerializeMovies(_cplusComedies, MovieTypes.Comedy);
            var result2 = _serializer.SerializeMovies(_hboComedies, MovieTypes.Comedy);
            var result3 = _serializer.SerializeMovies(_ncPremieresComediesDoc, MovieTypes.Comedy);

            Assert.True(result1.All(n => n.ProviderName == "CANAL+ VOD"));
            Assert.True(result2.All(n => n.ProviderName.ToLower().Contains("hbo")));
            Assert.True(result3.All(n => n.ProviderName.ToLower().Contains("premiery")));
        }

        [Test]
        public void SerializeFilmwebUrl_ShouldSerializeCorrectValues()
        {
            Arrange();
            var directors = new List<string>() { "Sean Mathias", "Test" };

            var result = _serializer.SerializeFilmwebUrl(_filmwebSearch2HtmlDoc, directors);

            Assert.AreEqual("https://www.filmweb.pl/film/Pi%C4%99tno-1997-31929", result);
        }

        [Test]
        public void SerializeFilmwebResult_ShouldSerializeCorrectValues()
        {
            Arrange();

            var title = "Ostateczna rozgrywka";
            var result = _serializer.SerializeFilmwebResult(_filmwebResultHtmlDoc, MovieTypes.Thriller, string.Empty, title);

            Assert.True(result.Title == title);
            Assert.True(result.FilmwebRating == 5.4m);
            Assert.True(result.Production == "Wielka Brytania");
            Assert.True(result.FilmwebFilmType == "Akcja, Thriller");
            Assert.True(result.FilmDescription.Contains("Piłkarski stadion zostaje opanowany"));
            Assert.True(result.Year == 2018);
            Assert.True(result.Cast.Contains("Dave Bautista"));
        }

        [Test]
        public void SerializeFilmwebResult_ShouldSerializeCorrectValuesForNewFilmwebResult()
        {
            Arrange();

            var title = "Kapitan Ameryka: Wojna bohaterów (2016)";
            var result = _serializer.SerializeFilmwebResult(_filmwebNewResultHtmlDoc, MovieTypes.Action, string.Empty, title);

            Assert.True(result.FilmwebRating == 7.5m);
            Assert.True(result.Production == "USA");
            Assert.True(result.FilmwebFilmType == "Akcja, Sci-Fi");
            Assert.True(result.FilmDescription.Contains("ONZ wprowadza przymusowy rejestr bohaterów"));
            Assert.True(result.Year == 2016);
            Assert.True(result.Cast.Contains("Robert Downey Jr."));
        }
    }
}
