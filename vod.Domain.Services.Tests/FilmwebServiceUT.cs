using AutoMapper;
using HtmlAgilityPack;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
using vod.Domain.Services.Utils.HtmlSource;
using vod.Domain.Services.Utils.HtmlSource.Serialize;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;

namespace vod.Domain.Services.Tests
{
    public class FilmwebServiceUT
    {
        private FilmwebService _fakeFilmwebService;
        private List<MovieEntity> _fakeStoredCollection;
        private Mock<IVodRepositoryBackground> _repoBackgroundMock;
        private FilmwebResult _expectedResult;
        private Mock<IHtmlSourceGetter> _htmlSourceGetterMock;
        private Mock<IHtmlSourceSerializer> _sourceSerializerMock;
        private Mock<IMapper> _mapperMock;
        private const string TEST_TITLE = "Test 1";

        void BaseArrange()
        {
            _expectedResult = new FilmwebResult() { Title = TEST_TITLE };
            _htmlSourceGetterMock = new Mock<IHtmlSourceGetter>();
            _sourceSerializerMock = new Mock<IHtmlSourceSerializer>();
            _sourceSerializerMock.Setup(x => x.SerializeOriginalTitle(It.IsAny<HtmlDocument>())).Returns("test");
            _sourceSerializerMock.Setup(x => x.SerializeDirectors(It.IsAny<HtmlDocument>())).Returns(new List<string>() { "test" });
            _sourceSerializerMock.Setup(x => x.SerializeFilmwebUrl(It.IsAny<HtmlDocument>(), It.IsAny<List<string>>())).Returns("testurl");
            _sourceSerializerMock.Setup(x => x.SerializeFilmwebResult(It.IsAny<HtmlDocument>(), It.IsAny<MovieTypes>(), It.IsAny<string>(), It.IsAny<string>())).Returns(_expectedResult);

            var resultModelMock = new Mock<MovieEntity>();
            resultModelMock.SetupAllProperties();
            resultModelMock.Object.Title = TEST_TITLE;

            _fakeStoredCollection = new List<MovieEntity> { resultModelMock.Object };
            _repoBackgroundMock = new Mock<IVodRepositoryBackground>();
            _repoBackgroundMock.Setup(x => x.GetResultsOfType(It.IsAny<int>())).Returns(_fakeStoredCollection);

            _mapperMock = new Mock<IMapper>();
            _mapperMock.Setup(x => x.Map<FilmwebResult>(It.IsAny<MovieEntity>())).Returns(_expectedResult);

            _fakeFilmwebService = new FilmwebService(
                _htmlSourceGetterMock.Object,
                _sourceSerializerMock.Object,
                _repoBackgroundMock.Object,
                _mapperMock.Object);
        }

        [Test]
        public void GetFilmwebResults_ShouldUseStoredDataIfExists()
        {
            BaseArrange();
            var movie = new NcPlusResult() { Title = TEST_TITLE };

            var result = _fakeFilmwebService.GetFilmwebResult(movie);

            _mapperMock.Verify(x => x.Map<FilmwebResult>(It.IsAny<MovieEntity>()));
            
        }

        [Test]
        public void GetFilmwebResults_SearchFilmwebResult()
        {
            BaseArrange();
            var movie = new NcPlusResult() { Title = It.IsAny<string>(), MoreInfoUrl="test" };

            var result = _fakeFilmwebService.GetFilmwebResult(movie);

            _htmlSourceGetterMock.Verify(n => n.GetHtmlFrom($"{NcPlusUrls.NcPlusGoUrl}{movie.MoreInfoUrl}"), Times.Once);
        }
        
        [Test]
        public void GetFilmwebResults_GetsStoredDataOnlyOncePerLifeScope()
        {
            BaseArrange();
            var movie = new NcPlusResult();

            for (int i = 0; i < 2; i++)
            {
                _fakeFilmwebService.GetFilmwebResult(movie);
            }

            _repoBackgroundMock.Verify(n => n.GetResultsOfType(It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void GetFilmwebResults_ReturnsNullIfFilmwebUrlIsEmpty()
        {
            BaseArrange();
            _sourceSerializerMock
                .Setup(x => x.SerializeFilmwebUrl(It.IsAny<HtmlDocument>(), It.IsAny<List<string>>()))
                .Returns(string.Empty);
            var movie = new NcPlusResult() { Title = It.IsAny<string>(), MoreInfoUrl = "test" };

            var result = _fakeFilmwebService.GetFilmwebResult(movie);

            Assert.IsNull(result);
        }
    }
}
