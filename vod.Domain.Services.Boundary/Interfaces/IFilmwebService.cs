using vod.Core.Boundary.Model;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IFilmwebService
    {
        Result CheckInFilmweb(Movie movie);
    }
}
