using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Commands;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;
using vod.SignalR.Hub.Hub;

namespace vod.Domain.Services.Tests
{
    public class StoredDataManagerUT
    {
        private Mock<IVodRepository> _repositoryMock;
        private Mock<IVodRepositoryBackground> _repositoryBgMock;
        private List<MovieEntity> _fakeStoredCollection;
        private Mock<IBackgroundWorker> _bgWorkerMock;
        private Mock<UpdateNotificationHub> _updateHubMock;
        private Mock<IHubCallerClients> _mockClients;
        private Mock<IRefreshDataService> _refreshDataServiceMock;
        private IStoredDataManager _storedDataManager;
        private StoredDataManager _storedDataManager2;
        private UseStorageIfPossibleCommand _cmd;
        private List<FilmwebResult> _fakeFilmwebResults;
        private BackgroundWorker _fakeBgWorker;

        private void BaseArrange()
        {
            var fakeResultModel = new MovieEntity() {Title = "test"};
            var fakeFilmwebResult = new FilmwebResult() {Title = "test" };
            _fakeStoredCollection = new List<MovieEntity>() {fakeResultModel};
            _fakeFilmwebResults = new List<FilmwebResult>() {fakeFilmwebResult};
            _repositoryMock = new Mock<IVodRepository>();
            _repositoryMock.Setup(x => x.GetStoredData(It.IsAny<int>())).Returns(_fakeStoredCollection.AsQueryable());

            _repositoryBgMock = new Mock<IVodRepositoryBackground>();

            _bgWorkerMock = new Mock<IBackgroundWorker>();

            var stateManager = new Mock<IBackgroundWorkerStateManager>();
            _fakeBgWorker = new BackgroundWorker(stateManager.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<FilmwebResult>(fakeResultModel)).Returns(new FilmwebResult() {Title = fakeResultModel.Title, StoredDate = fakeResultModel.StoredDate});

            _updateHubMock = new Mock<UpdateNotificationHub>();
            var fakeUpdateHub = new UpdateNotificationHub();
            _mockClients = new Mock<IHubCallerClients>();
            _mockClients.Setup(x => x.All).Returns(new Mock<IClientProxy>().Object).Verifiable();
            fakeUpdateHub.Clients = _mockClients.Object;

            _refreshDataServiceMock = new Mock<IRefreshDataService>();
            _refreshDataServiceMock.Setup(x => x.Refresh(It.IsAny<MovieTypes>()));

            _storedDataManager = new StoredDataManager(mapperMock.Object, _repositoryMock.Object, _bgWorkerMock.Object, _refreshDataServiceMock.Object);
            _storedDataManager2 = new StoredDataManager(mapperMock.Object, _repositoryMock.Object, _fakeBgWorker, _refreshDataServiceMock.Object);

            _cmd = new UseStorageIfPossibleCommand()
            {
                Type = MovieTypes.Action
            };
        }

        [Test]
        public void UseStorageIfPossible_ShouldReturnDataFromRepository()
        {
            BaseArrange();

            var result = _storedDataManager.UseStorageIfPossible(_cmd).ToList();

            Assert.AreEqual(result.First().Title, _fakeFilmwebResults.First().Title);
            Assert.AreEqual(result.First().StoredDate, _fakeFilmwebResults.First().StoredDate);
        }

        [Test]
        public void UseStorageIfPossible_ShouldNotUseBgWorkerIfDoesNotHaveTo()
        {
            BaseArrange();
            _repositoryMock.Setup(x => x.GetUpdateDateTime(It.IsAny<int>())).Returns(DateTime.Now);

            _storedDataManager.UseStorageIfPossible(_cmd);

            _bgWorkerMock.Verify(n => n.Execute(MovieTypes.Action, It.IsAny<Func<bool>>()), Times.Never);
        }

        [Test]
        public void UseStorageIfPossible_ShouldCallExecuteIfRefreshIsNeeded()
        {
            BaseArrange();
            _repositoryMock.Setup(x => x.GetUpdateDateTime(It.IsAny<int>())).Returns(DateTime.Now.AddDays(-2));

            _storedDataManager.UseStorageIfPossible(_cmd);

            _bgWorkerMock.Verify(n => n.Execute(MovieTypes.Action, It.IsAny<Func<bool>>()), Times.Once);
        }

        [Test]
        public void UseStorageIfPossible_ShouldCallRefreshDataServiceIfNeeded()
        {
            BaseArrange();
            _repositoryMock.Setup(x => x.GetUpdateDateTime(It.IsAny<int>())).Returns(DateTime.Now.AddDays(-2));

            _storedDataManager2.UseStorageIfPossible(_cmd);

            _refreshDataServiceMock.Verify(n => n.Refresh(_cmd.Type), Times.Once);
        }
    }
}
