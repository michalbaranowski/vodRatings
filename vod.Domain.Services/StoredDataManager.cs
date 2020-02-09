using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using vod.Domain.Services.Boundary.Commands;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;
using vod.SignalR.Hub.Hub;

namespace vod.Domain.Services
{
    public class StoredDataManager : IStoredDataManager
    {
        private readonly IMapper _mapper;
        private readonly IVodRepository _repository;
        private readonly IVodRepositoryBackground _repositoryBackground;
        private readonly IBackgroundWorker _backgroundWorker;
        private readonly UpdateNotificationHub _notificationHub;

        public StoredDataManager(
            IMapper mapper,
            IVodRepository repository,
            IVodRepositoryBackground repositoryBackground,
            IBackgroundWorker backgroundWorker,
            UpdateNotificationHub notificationHub)
        {
            _mapper = mapper;
            _repository = repository;
            _repositoryBackground = repositoryBackground;
            _backgroundWorker = backgroundWorker;
            _notificationHub = notificationHub;
        }

        public IEnumerable<FilmwebResult> UseStorageIfPossible(UseStorageIfPossibleCommand command)
        {
            var storedCollection = _repository.GetStoredData((int)command.Type).ToList();
            var alreadyWatched = _repository.GetAlreadyWatched(command.Username);

            foreach (var item in alreadyWatched)
            {
                var movie = storedCollection.FirstOrDefault(n => n.Title == item.Title);

                if (movie == null)
                    continue;

                movie.IsAlreadyWatched = true;
            }

            UseStorageInternal(storedCollection, command.Type, command.Func);
            return storedCollection.Select(n => _mapper.Map<FilmwebResult>(n));
        }

        private void UseStorageInternal(
            List<ResultModel> storedCollection,
            MovieTypes type,
            Func<IEnumerable<FilmwebResult>> func)
        {
            if ((!storedCollection.Any() ||
                 storedCollection.FirstOrDefault()?.RefreshDate < DateTime.Now.AddDays(-1)))
            {
                _backgroundWorker.Execute(type, () =>
                {
                    _notificationHub.NotifyRefreshStarted(type);
                    var results = func().ToList();
                    var entities = results.Select(n => _mapper.Map<ResultModel>(n));
                    _repositoryBackground.RefreshData(entities, (int)type);
                    _notificationHub.NotifyUpdate(type);
                    return true;
                });
            }

        }
    }
}
