using System;
using System.Collections.Generic;
using System.Text;
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
        }

        public IEnumerable<ResultModel> GetStoredData()
        {
            return _context.Results;
        }
    }
}
