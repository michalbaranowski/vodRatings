using System;
using System.Collections.Generic;
using System.Linq;
using vod.Repository.Boundary;
using vod.Repository.Boundary.Models;

namespace vod.Repository
{
    public class VodRepository : IVodRepository
    {
        private readonly IAppDbContext _context;

        public VodRepository(IAppDbContext context)
        {
            _context = context;
            _context.EnsureCreated();
        }

        public void RefreshData(IEnumerable<ResultModel> results)
        {
            _context.Results.RemoveRange(_context.Results);
            _context.SaveChanges();

            _context.Results.AddRange(results);
            _context.SaveChanges();
        }

        public IEnumerable<ResultModel> GetStoredData()
        {
            return _context.Results;
        }
    }
}
