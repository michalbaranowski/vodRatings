using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public class AppDbContext : IdentityDbContext<IdentityUser>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }
        public DbSet<MovieEntity> Movies { get; set; }

        public DbSet<BlackListedMovieEntity> BlackListedMovies { get; set; }

        public DbSet<UserMovieEntity> UserMovies { get; set; }

        public DbSet<UpdateLogEntity> UpdateLogs { get; set; }

        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }
    }
}
