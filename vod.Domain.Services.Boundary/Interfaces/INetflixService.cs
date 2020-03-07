using vod.Domain.Services.Boundary.Interfaces;

namespace vod.Domain.Services
{
    public interface INetflixService : IMoviesGetter<NetflixResult>
    {
    }
}