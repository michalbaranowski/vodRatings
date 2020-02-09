using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary
{
    public interface IAlreadyWatchedFilmService
    {
        void Add(AlreadyWatchedMovie movie);
    }
}
