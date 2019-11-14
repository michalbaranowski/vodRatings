using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Serialize;

namespace vod.Domain.Services.Tests
{
    public class NcPlusServiceUT
    {
        private Mock<IHtmlSourceGetter> _sourceGetterMock;
        private Mock<IHtmlSourceSerializer> _serializerMock;
        private NcPlusService _ncPlusService;
        private UrlGetter _urlGetterFake;


        private void BaseArrange()
        {
            _sourceGetterMock = new Mock<IHtmlSourceGetter>();

            _serializerMock = new Mock<IHtmlSourceSerializer>();
            _serializerMock.SetupSequence(x => x.SerializeMovies(It.IsAny<HtmlDocument>(), It.IsAny<MovieTypes>()))
                .Returns(new List<Movie>() {new Movie() {Title = "title1"}})
                .Returns(new List<Movie>() {new Movie() {Title = "title2"}, new Movie() {Title = "title2"}})
                .Returns(new List<Movie>() {new Movie() {Title = "title3"}})
                .Returns(new List<Movie>() {new Movie() {Title = "title4"}})
                .Returns(new List<Movie>() {new Movie() {Title = "title5"}});

            _urlGetterFake = new UrlGetter();
            
            _ncPlusService = new NcPlusService(_sourceGetterMock.Object, _serializerMock.Object, _urlGetterFake);
        }

        [Test]
        public void GetMoviesOfType_ShouldReturnDataFromAllSources()
        {
            BaseArrange();
            var results = _ncPlusService.GetMoviesOfType(MovieTypes.Thriller);
            Assert.True(results.Count() == 5);
        }

        [Test]
        public void GetMoviesOfType_ShouldReturnResultsWithoutDupes()
        {
            BaseArrange();
            var results = _ncPlusService.GetMoviesOfType(MovieTypes.Thriller);
            Assert.False(results.GroupBy(x=>x.Title).Any(n=>n.Count() > 1));
        }
    }
}
