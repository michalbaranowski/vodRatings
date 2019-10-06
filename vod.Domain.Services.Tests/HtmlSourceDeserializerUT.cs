using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using NUnit.Framework;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Tests.Resources;
using vod.Domain.Services.Utils.HtmlSource.Deserialize;

namespace vod.Domain.Services.Tests
{
    public class HtmlSourceDeserializerUT
    {
        private HtmlSourceDeserializer _deserializer;
        private HtmlDocument _moviesHtmlDoc;
        private HtmlDocument _moreInfoHtmlDoc;
        private HtmlDocument _filmwebResultHtmlDoc;
        private HtmlDocument _cplusComedies;
        private HtmlDocument _ncPremieresComediesDoc;
        private HtmlDocument _hboComedies;


        private void Arrange()
        {
            _deserializer = new HtmlSourceDeserializer();
            
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
        }

        [Test]
        public void DeserializeMovies_ShouldDeserializeCorrectValues()
        {
            Arrange();

            var result = _deserializer.DeserializeMovies(_moviesHtmlDoc, MovieTypes.Thriller);

            Assert.True(result.Any(n=>n.Title == "Ostateczna rozgrywka"));
            Assert.True(result.Any(n => n.MoreInfoUrl == "/Collection/Asset?codename=ostateczna-rozgrywka-19"));
            Assert.True(result.Any(n => n.MovieType == MovieTypes.Thriller));
        }

        [Test]
        public void DeserializeMovies_ShouldDeserializeCorrectProviderName()
        {
            Arrange();

            var result1 = _deserializer.DeserializeMovies(_cplusComedies, MovieTypes.Comedy);
            var result2 = _deserializer.DeserializeMovies(_hboComedies, MovieTypes.Comedy);
            var result3 = _deserializer.DeserializeMovies(_ncPremieresComediesDoc, MovieTypes.Comedy);

            Assert.True(result1.All(n => n.ProviderName == "CANAL+ VOD"));
            Assert.True(result2.All(n => n.ProviderName.ToLower().Contains("hbo")));
            Assert.True(result3.All(n => n.ProviderName.ToLower().Contains("premiery")));
        }

        [Test]
        public void DeserializeFilmwebUrl_ShouldDeserializeCorrectValues()
        {
            Arrange();

            var result = _deserializer.DeserializeFilmwebUrl(_moreInfoHtmlDoc);

            Assert.AreEqual("http://www.filmweb.pl/film/ostateczna+rozgrywka+19-2018-780203", result);
        }

        [Test]
        public void DeserializeFilmwebResult_ShouldDeserializeCorrectValues()
        {
            Arrange();

            var result = _deserializer.DeserializeFilmwebResult(_filmwebResultHtmlDoc, MovieTypes.Thriller);

            Assert.True(result.Title == "Ostateczna rozgrywka");
            Assert.True(result.FilmwebRating == 5.4m);
            Assert.True(result.Year == 2018);
        }
    }
}
