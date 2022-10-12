using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services
{
    public class FilmResultWithMovieType : FilmResult
    {
        public MovieTypes MovieType { get; set; }
    }
}