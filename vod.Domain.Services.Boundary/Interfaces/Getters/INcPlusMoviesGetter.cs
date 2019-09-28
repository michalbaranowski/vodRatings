using System.Collections.Generic;
using System.Threading.Tasks;
using vod.Domain.Services.Boundary.Interfaces.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary.Interfaces.Getters
{
    public interface INcPlusMoviesGetter
    {
        Task<IEnumerable<Movie>> GetMoviesOfType(MovieTypes type);
    }
}
