using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IAppDbContext
    {
        DbSet<MovieEntity> Movies { get; set; }
        DbSet<UserMovieEntity> UserMovies { get; }
        DbSet<UpdateLogEntity> UpdateLogs { get; set; }
        DbSet<BlackListedMovieEntity> BlackListedMovies { get; set; }

        int SaveChanges();
        void AddRange(IEnumerable<object> entities);
        void EnsureCreated();
    }
}