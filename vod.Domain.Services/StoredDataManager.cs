using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using vod.Domain.Services.Boundary.Interfaces;
using vod.Domain.Services.Boundary.Models;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;

namespace vod.Domain.Services
{
    public class StoredDataManager : IStoredDataManager
    {
        private readonly IMapper _mapper;
        private readonly IVodRepository _repository;
        private readonly IVodRepositoryBackground _repositoryBackground;
        private readonly IBackgroundWorker _backgroundWorker;

        public StoredDataManager(
            IMapper mapper,
            IVodRepository repository,
            IVodRepositoryBackground repositoryBackground,
            IBackgroundWorker backgroundWorker)
        {
            _mapper = mapper;
            _repository = repository;
            _repositoryBackground = repositoryBackground;
            _backgroundWorker = backgroundWorker;
        }

        public IEnumerable<FilmwebResult> UseStorageIfPossible(Func<IEnumerable<FilmwebResult>> func)
        {
            var storedCollection = _repository.GetStoredData().ToList();

            if ((!storedCollection.Any() ||
                storedCollection.FirstOrDefault()?.StoredDate < DateTime.Now.AddDays(-1)))
            {
                _backgroundWorker.Execute(() =>
                {
                    var results = func().ToList();
                    var entities = results.Select(n => _mapper.Map<ResultModel>(n));
                    _repositoryBackground.RefreshData(entities);
                    return true;
                });
            }

            return storedCollection.Select(n => _mapper.Map<FilmwebResult>(n));
        }
    }
}
