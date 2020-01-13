using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using Moq;
using NUnit.Framework;
using vod.Domain.Services.Boundary;
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
        private List<ResultModel> _fakeStoredCollection;
        private Mock<IBackgroundWorker> _bgWorkerMock;
        private Mock<UpdateNotificationHub> _updateHubMock;
        private IStoredDataManager _storedDataManager;
        private StoredDataManager _storedDataManager2;
        private List<FilmwebResult> _fakeFilmwebResults;
        private BackgroundWorker _fakeBgWorker;

        private void BaseArrange(DateTime? fakeStoredDate = null)
        {
            var dateNow = DateTime.Now;
            var fakeResultModel = new ResultModel() {Title = "test", StoredDate = fakeStoredDate ?? dateNow, RefreshDate = fakeStoredDate ?? dateNow};
            var fakeFilmwebResult = new FilmwebResult() {Title = "test", StoredDate = dateNow };
            _fakeStoredCollection = new List<ResultModel>() {fakeResultModel};
            _fakeFilmwebResults = new List<FilmwebResult>() {fakeFilmwebResult};
            _repositoryMock = new Mock<IVodRepository>();
            _repositoryMock.Setup(x => x.GetStoredData(It.IsAny<int>())).Returns(_fakeStoredCollection);

            _repositoryBgMock = new Mock<IVodRepositoryBackground>();

            _bgWorkerMock = new Mock<IBackgroundWorker>();

            var stateManager = new Mock<IBackgroundWorkerStateManager>();
            _fakeBgWorker = new BackgroundWorker(stateManager.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<FilmwebResult>(fakeResultModel)).Returns(new FilmwebResult() {Title = fakeResultModel.Title, StoredDate = fakeResultModel.StoredDate});

            _updateHubMock = new Mock<UpdateNotificationHub>();
            _updateHubMock.Setup(x => x.Clients).Returns(new Mock<IHubCallerClients>().Object);

            _storedDataManager = new StoredDataManager(mapperMock.Object, _repositoryMock.Object, _repositoryBgMock.Object, _bgWorkerMock.Object, _updateHubMock.Object);
            _storedDataManager2 = new StoredDataManager(mapperMock.Object, _repositoryMock.Object, _repositoryBgMock.Object, _fakeBgWorker, _updateHubMock.Object);
        }

        [Test]
        public void UseStorageIfPossible_ShouldReturnDataFromRepository()
        {
            BaseArrange();

            var result = _storedDataManager.UseStorageIfPossible(MovieTypes.Action, () => _fakeFilmwebResults).ToList();

            Assert.AreEqual(result.First().Title, _fakeFilmwebResults.First().Title);
            Assert.AreEqual(result.First().StoredDate, _fakeFilmwebResults.First().StoredDate);
        }

        [Test]
        public void UseStorageIfPossible_ShouldNotUseBgWorkerIfDoesNotHaveTo()
        {
            BaseArrange();

            _storedDataManager.UseStorageIfPossible(MovieTypes.Action, () => _fakeFilmwebResults);

            _bgWorkerMock.Verify(n => n.Execute(MovieTypes.Action, It.IsAny<Func<bool>>()), Times.Never);
        }

        [Test]
        public void UseStorageIfPossible_ShouldCallExecuteIfRefreshIsNeeded()
        {
            BaseArrange(DateTime.Now.AddDays(-2));

            _storedDataManager.UseStorageIfPossible(MovieTypes.Action, () => _fakeFilmwebResults);

            _bgWorkerMock.Verify(n => n.Execute(MovieTypes.Action, It.IsAny<Func<bool>>()), Times.Once);
        }

        [Test]
        public void UseStorageIfPossible_ShouldCallRepositoryBackgroundIfRefreshNeeded()
        {
            BaseArrange(DateTime.Now.AddDays(-2));

            _storedDataManager2.UseStorageIfPossible(MovieTypes.Action, () => _fakeFilmwebResults);

            _repositoryBgMock.Verify(n => n.RefreshData(It.IsAny<IEnumerable<ResultModel>>(), It.IsAny<int>()), Times.Once);
        }

        [Test]
        public void UseStorageIfPossible_ShouldCallNotifyUpdateIfRefreshNeeded()
        {
            BaseArrange(DateTime.Now.AddDays(-2));

            _storedDataManager2.UseStorageIfPossible(MovieTypes.Action, () => _fakeFilmwebResults);

            _updateHubMock.Verify(n => n.NotifyUpdate(It.IsAny<MovieTypes>()), Times.Once);
        }
    }
}
