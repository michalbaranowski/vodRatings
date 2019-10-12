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
        }

        public IEnumerable<ResultModel> GetStoredData(int type)
        {
            return _context.Results.Where(n=>n.VodFilmType == type);
        }
    }
}
