using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using vod.Domain.Services.Boundary.Commands;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;
using vod.Domain.Services.Utils;
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
        private readonly IRefreshDataService _refreshDataService;
        private readonly UpdateNotificationHub _notificationHub;

        public StoredDataManager(
            IMapper mapper,
            IVodRepository repository,
            IBackgroundWorker backgroundWorker,
            IRefreshDataService refreshDataService)
        {
            _mapper = mapper;
            _repository = repository;
            _backgroundWorker = backgroundWorker;
            _refreshDataService = refreshDataService;
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

            RefreshIfNeeded(storedCollection, command.Type, command.CrawlFunc);
            return storedCollection.Select(n => _mapper.Map<FilmwebResult>(n));
        }

        private void RefreshIfNeeded(
            List<ResultModel> storedCollection,
            MovieTypes type,
            Func<IEnumerable<FilmwebResult>> func)
        {
            if (storedCollection.Any() == false || storedCollection.NeedRefresh())
            {
                _backgroundWorker.Execute(type, () => _refreshDataService.Refresh(type, func));
            }

        }
    }
}
