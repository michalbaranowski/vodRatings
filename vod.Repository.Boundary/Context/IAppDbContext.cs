using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IAppDbContext
    {
        DbSet<ResultModel> Results { get; set; }
        DbSet<AlreadyWatchedModel> AlreadyWatched { get; }

        int SaveChanges();
        void AddRange(IEnumerable<object> entities);
        void EnsureCreated();
    }
}