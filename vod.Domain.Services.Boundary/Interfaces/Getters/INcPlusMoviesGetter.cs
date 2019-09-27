using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using vod.Domain.Services.Boundary.Enums;
using vod.Domain.Services.Boundary.Models;

namespace vod.Domain.Services.Boundary
{
    public interface INcPlusMoviesGetter
    {
        Task<IEnumerable<Movie>> GetMoviesOfType(MovieTypes type);
    }
}
