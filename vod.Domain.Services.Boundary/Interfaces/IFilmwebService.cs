using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IFilmwebService
    {
        FilmwebResult GetFilmwebResult(Movie movie);
    }
}
