using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;

namespace vod.Repository
{
    public class VodRepositoryBackground : IVodRepositoryBackground
    {
        private readonly DbContextOptions<AppDbContext> _opt;

        public VodRepositoryBackground(DbContextOptions<AppDbContext> opt)
        {
            _opt = opt;
        }

        public void RefreshData(IEnumerable<ResultModel> results)
        {
            using (var ctx = new AppDbContext(_opt))
            {
                ctx.Results.RemoveRange(ctx.Results);
                ctx.SaveChanges();

                var resultsList = results.ToList();
                foreach (var result in resultsList) result.StoredDate = DateTime.Now;

                ctx.Results.AddRange(resultsList);
                ctx.SaveChanges();
            }
        }
    }
}
