using System.Collections.Generic;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IMoviesGetter<T>
    {
        IEnumerable<T> GetMoviesOfType(MovieTypes type);
    }
}