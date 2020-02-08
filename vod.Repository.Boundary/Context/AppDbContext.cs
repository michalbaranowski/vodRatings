using Microsoft.EntityFrameworkCore;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt)
        {
            
        }
        public DbSet<ResultModel> Results { get; set; }
        public void EnsureCreated()
        {
            Database.EnsureCreated();
        }
    }
}
