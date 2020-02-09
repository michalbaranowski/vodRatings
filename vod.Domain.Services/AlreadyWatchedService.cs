using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using vod.Domain.Services.Boundary;
using vod.Domain.Services.Boundary.Models;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;

namespace vod.Domain.Services
{
    public class AlreadyWatchedService : IAlreadyWatchedFilmService
    {
        private IVodRepository _repository;
        private IMapper _mapper;

        public AlreadyWatchedService(IVodRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public void Add(AlreadyWatchedMovie movie)
        {
            _repository.AddAlreadyWatched(_mapper.Map<AlreadyWatchedModel>(movie));
        }
    }
}
