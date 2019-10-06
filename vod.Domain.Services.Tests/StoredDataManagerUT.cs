using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using NUnit.Framework;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;

namespace vod.Domain.Services.Tests
{
    public class StoredDataManagerUT
    {
        private Mock<IVodRepository> _repositoryMock;
        private Mock<IVodRepositoryBackground> _repositoryBgMock;
        private List<ResultModel> _fakeStoredCollection;
        private Mock<IBackgroundWorker> _bgWorkerMock;
        private IStoredDataManager _storedDataManager;
        private List<FilmwebResult> _fakeFilmwebResults;

        private void BaseArrange(DateTime? fakeStoredDate = null)
        {
            var dateNow = DateTime.Now;
            var fakeResultModel = new ResultModel() {Title = "test", StoredDate = fakeStoredDate ?? dateNow};
            var fakeFilmwebResult = new FilmwebResult() {Title = "test", StoredDate = dateNow};
            _fakeStoredCollection = new List<ResultModel>() {fakeResultModel};
            _fakeFilmwebResults = new List<FilmwebResult>() {fakeFilmwebResult};
            _repositoryMock = new Mock<IVodRepository>();
            _repositoryMock.Setup(x => x.GetStoredData(It.IsAny<int>())).Returns(_fakeStoredCollection);

            _repositoryBgMock = new Mock<IVodRepositoryBackground>();

            _bgWorkerMock = new Mock<IBackgroundWorker>();
            var mapperMock = new Mock<IMapper>();
            mapperMock.Setup(x => x.Map<FilmwebResult>(fakeResultModel)).Returns(new FilmwebResult() {Title = fakeResultModel.Title, StoredDate = fakeResultModel.StoredDate});

            _storedDataManager = new StoredDataManager(mapperMock.Object, _repositoryMock.Object, _repositoryBgMock.Object, _bgWorkerMock.Object);
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
        public void UseStorageIfPossible_ShouldUseBgWorkerIStoredDateIsOld()
        {
            BaseArrange(DateTime.Now.AddDays(-2));

            _storedDataManager.UseStorageIfPossible(MovieTypes.Action, () => _fakeFilmwebResults);

            _bgWorkerMock.Verify(n => n.Execute(MovieTypes.Action, It.IsAny<Func<bool>>()), Times.Once);
        }
    }
}
