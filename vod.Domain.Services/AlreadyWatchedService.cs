using AutoMapper;
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

        public void Add(AlreadyWatchedMovie userMovie)
        {
            _repository.AddAlreadyWatched(_mapper.Map<UserMovieEntity>(userMovie));
        }

        public void RemoveAt(int movieId, string userId)
        {
            _repository.RemoveAlreadyWatched(movieId, userId);
        }
    }
}
