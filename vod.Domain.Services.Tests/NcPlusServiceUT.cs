using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Deserialize;

namespace vod.Domain.Services.Tests
{
    public class NcPlusServiceUT
    {
        private Mock<IHtmlSourceGetter> _sourceGetterMock;
        private Mock<IHtmlSourceDeserializer> _deserializerMock;
        private NcPlusService _ncPlusService;

        private void BaseArrange()
        {
            _sourceGetterMock = new Mock<IHtmlSourceGetter>();
            _deserializerMock = new Mock<IHtmlSourceDeserializer>();
            _deserializerMock.SetupSequence(x => x.DeserializeMovies(It.IsAny<HtmlDocument>(), It.IsAny<MovieTypes>()))
                .Returns(new List<Movie>() {new Movie() {Title = "title1"}})
                .Returns(new List<Movie>() {new Movie() {Title = "title2"}, new Movie() {Title = "title2"}})
                .Returns(new List<Movie>() {new Movie() {Title = "title3"}});
            
            _ncPlusService = new NcPlusService(_sourceGetterMock.Object, _deserializerMock.Object);
        }

        [Test]
        public void GetMoviesOfType_ShouldReturnDataFromAllSources()
        {
            BaseArrange();
            var results = _ncPlusService.GetMoviesOfType(MovieTypes.Thriller);
            Assert.True(results.Count() == 3);
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
