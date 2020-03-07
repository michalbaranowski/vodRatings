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
        private IList<string> _urlsCollection;
        private Mock<UrlGetter> _urlGetterMock;

        private void BaseArrange()
        {
            _sourceGetterMock = new Mock<IHtmlSourceGetter>();

            _serializerMock = new Mock<IHtmlSourceSerializer>();
            _serializerMock.SetupSequence(x => x.SerializeMoviesNcPlus(It.IsAny<HtmlDocument>(), It.IsAny<MovieTypes>()))
                .Returns(new List<NcPlusResult>() { new NcPlusResult() { Title = "title1" } })
                .Returns(new List<NcPlusResult>() { new NcPlusResult() { Title = "title2" }, new NcPlusResult() { Title = "title2" } })
                .Returns(new List<NcPlusResult>() { new NcPlusResult() { Title = "title3" } })
                .Returns(new List<NcPlusResult>() { new NcPlusResult() { Title = "title4" } })
                .Returns(new List<NcPlusResult>() { new NcPlusResult() { Title = "title5" } })
                .Returns(new List<NcPlusResult>() { new NcPlusResult() { Title = "title1" } })
                .Returns(new List<NcPlusResult>() { new NcPlusResult() { Title = "title2" }, new NcPlusResult() { Title = "title2" } })
                .Returns(new List<NcPlusResult>() { new NcPlusResult() { Title = "title3" } })
                .Returns(new List<NcPlusResult>() { new NcPlusResult() { Title = "title4" } })
                .Returns(new List<NcPlusResult>() { new NcPlusResult() { Title = "title5" } });

            var realUrlGetter = new UrlGetter();
            _urlsCollection = realUrlGetter.GetNcPlusBaseUrls().ToList();
            _urlGetterMock = new Mock<UrlGetter>();
            
            _ncPlusService = new NcPlusService(_sourceGetterMock.Object, _serializerMock.Object, _urlGetterMock.Object);
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

        [Test]
        public void GetMoviesOfType_UseStoredDataIfPossible()
        {
            BaseArrange();

            _ncPlusService.GetMoviesOfType(MovieTypes.Thriller);
            _ncPlusService.GetMoviesOfType(MovieTypes.Thriller);

            _sourceGetterMock.Verify(x => x.GetHtmlFrom(It.IsAny<string>()), Times.Exactly(_urlsCollection.Count()));
        }

        [Test]
        public void GetMoviesOfType_DoNotUseStoredDataIfItsAnotherMovieType()
        {
            BaseArrange();

            _ncPlusService.GetMoviesOfType(MovieTypes.Thriller);
            _ncPlusService.GetMoviesOfType(MovieTypes.Action);

            _sourceGetterMock.Verify(x => x.GetHtmlFrom(It.IsAny<string>()), Times.Exactly(_urlsCollection.Count() * 2));
        }
    }
}
