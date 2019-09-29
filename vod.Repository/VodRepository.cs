using System.Collections.Generic;
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

        public void SaveData(IEnumerable<ResultModel> results)
        {
            _context.AddRange(results);
            _context.SaveChanges();
        }

        public IEnumerable<ResultModel> GetStoredData()
        {
            return _context.Results;
        }
    }
}
