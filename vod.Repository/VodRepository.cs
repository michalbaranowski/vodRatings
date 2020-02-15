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

        public void AddAlreadyWatched(AlreadyWatchedModel movie)
        {
            if (_context.AlreadyWatched.Any(n => n.Title == movie.Title && n.Username == movie.Username))
                return;

            _context.AlreadyWatched.Add(movie);
            _context.SaveChanges();
        }

        public void RemoveAlreadyWatched(AlreadyWatchedModel alreadyWatchedModel)
        {
            var objToRemove = _context.AlreadyWatched.FirstOrDefault(n => n.Title == alreadyWatchedModel.Title);

            if (objToRemove == null)
                return;

            _context.AlreadyWatched.Remove(objToRemove);
            _context.SaveChanges();
        }

        public IEnumerable<AlreadyWatchedModel> GetAlreadyWatched(string username)
        {
            return _context.AlreadyWatched.Where(n => n.Username == username);
        }

        public IEnumerable<MovieEntity> GetStoredData(int type)
        {
            return _context.Movies.Where(n=>n.VodFilmType == type);
        }
    }
}
