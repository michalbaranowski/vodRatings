using Microsoft.EntityFrameworkCore;
using System;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }
        public DbSet<MovieEntity> Movies { get; set; }

        [Obsolete("Do wywalenia docelowo")]
        public DbSet<AlreadyWatchedModel> AlreadyWatched { get; set; }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }
    }
}
