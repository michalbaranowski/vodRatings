using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;
using vod.SignalR.Hub.Hub;

namespace vod.Domain.Services.Tests
{
    public class RefreshDataServiceUT
    {
        private Mock<IRefreshStateService> _refreshStateService;
        private Mock<IMapper> _mapperMock;
        private Mock<INetflixService> _netflixService;
        private Mock<IFilmwebResultsProvider> _filmwebResultsProvider;
        private Mock<ICanalPlusService> _canalPlusService;

        public Mock<IDisneyPlusService> _disneyPlusService { get; private set; }

        private Mock<IVodRepositoryBackground> _repoBackground;
        private Mock<UpdateNotificationHub> _hubMock;
        private IRefreshDataService _refreshDataService;
        private Mock<IHubCallerClients> _mockClients;

        private static MovieTypes _expectedMovieType = MovieTypes.Action;
        private List<MovieEntity> _fakeMovieEntities = new List<MovieEntity>() { new MovieEntity() { Title="test", VodFilmType = (int)_expectedMovieType } };
        private List<BlackListedMovieEntity> _fakeBlackListedMovieEntities = new List<BlackListedMovieEntity>() { new BlackListedMovieEntity() { Title="test2" } };
        private List<FilmwebResult> _fakeFilmwebResults = new List<FilmwebResult>() { new FilmwebResult() { Title = "test" } };

        private void BaseArrange()
        {
            _refreshStateService = new Mock<IRefreshStateService>();
            _mapperMock = new Mock<IMapper>();

            _netflixService = new Mock<INetflixService>();
            _canalPlusService = new Mock<ICanalPlusService>();
            _disneyPlusService = new Mock<IDisneyPlusService>();

            _repoBackground = new Mock<IVodRepositoryBackground>();
            _repoBackground.Setup(x => x.GetResultsOfType((int)_expectedMovieType)).Returns(_fakeMovieEntities);
            _repoBackground.Setup(x => x.GetBlackListedMovies()).Returns(_fakeBlackListedMovieEntities);

            _hubMock = new Mock<UpdateNotificationHub>();            
            _mockClients = new Mock<IHubCallerClients>();
            _mockClients.Setup(x => x.All).Returns(new Mock<IClientProxy>().Object).Verifiable();
            _hubMock.Object.Clients = _mockClients.Object;

            _refreshDataService = new RefreshDataService(
                _refreshStateService.Object,
                _mapperMock.Object,
                _netflixService.Object,
                _canalPlusService.Object,
                _disneyPlusService.Object,
                _filmwebResultsProvider.Object,
                _repoBackground.Object,
                _hubMock.Object);
        }

        private void Act() => _refreshDataService.Refresh(_expectedMovieType);

        [Test]
        public void Refresh_ShouldNotifyThatRefreshingStarted()
        {
            BaseArrange();

            Act();

            _mockClients.Verify(x => x.All, Times.AtLeastOnce);
        }

        [Test]
        public void Refresh_ShouldCallSetCurrentRefreshState()
        {
            BaseArrange();

            Act();

            _refreshStateService.Verify(x => x.SetCurrentRefreshState(_expectedMovieType), Times.Once);
        }

        [Test]
        public void Refresh_ShouldCallGetResultsOfType()
        {
            BaseArrange();

            Act();

            _repoBackground.Verify(x => x.GetResultsOfType((int)_expectedMovieType), Times.Once);
        }

        [Test]
        public void Refresh_ShouldCallGetBlacklistedMovies()
        {
            BaseArrange();

            Act();

            _repoBackground.Verify(x => x.GetBlackListedMovies(), Times.Once);
        }

        [Test]
        public void Refresh_ShouldCallGetFilmwebResultsByNcPlusResults()
        {
            BaseArrange();

            Act();

            _filmwebResultsProvider.Verify(x => x.GetFilmwebResultsByBaseResults(It.IsAny<IEnumerable<FilmResultWithMovieType>>()), Times.Once);
        }

        [Test]
        public void Refresh_ShouldCallMarkAsDeleted()
        {
            BaseArrange();

            Act();

            _repoBackground.Verify(x => x.MarkAsDeleted(It.IsAny<IEnumerable<MovieEntity>>()), Times.Once);
        }

        [Test]
        public void Refresh_ShouldCallAddMovies()
        {
            BaseArrange();

            Act();

            _repoBackground.Verify(x => x.AddMovies(It.IsAny<IEnumerable<MovieEntity>>()), Times.Once);
        }

        [Test]
        public void Refresh_ShouldCallAddBlacklistedMovies()
        {
            BaseArrange();

            Act();

            _repoBackground.Verify(x => x.AddBlackListedMovies(It.IsAny<IEnumerable<BlackListedMovieEntity>>()), Times.Once);
        }

        [Test]
        public void Refresh_ShouldCallLogUpdate()
        {
            BaseArrange();

            Act();

            _repoBackground.Verify(x => x.LogUpdate((int)_expectedMovieType), Times.Once);
        }

        [Test]
        public void Refresh_ShouldCallRemoveCurrentRefreshState()
        {
            BaseArrange();

            Act();

            _refreshStateService.Verify(x => x.RemoveCurrentRefreshState(), Times.Once);
        }
    }
}
