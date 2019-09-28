using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IFilmwebUrlGetter
    {
        string GetFilmwebUrl(Movie movie);
    }
}
