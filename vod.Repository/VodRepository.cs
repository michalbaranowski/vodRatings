using System;
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

        public void AddAlreadyWatched(UserMovieEntity userMovie)
        {
            if (_context.UserMovies.Any(n => n.UserId == userMovie.UserId && n.MovieId == userMovie.MovieId))
                return;

            _context.UserMovies.Add(userMovie);
            _context.SaveChanges();
        }

        public void RemoveAlreadyWatched(int movieId, string userId)
        {
            var objToRemove = _context.UserMovies.FirstOrDefault(n => n.MovieId == movieId && n.UserId == userId);

            if (objToRemove == null)
                return;

            _context.UserMovies.Remove(objToRemove);
            _context.SaveChanges();
        }

        public IQueryable<UserMovieEntity> GetAlreadyWatched(string userId)
        {
            return _context.UserMovies.Where(n => n.UserId == userId);
        }

        public IQueryable<MovieEntity> GetStoredData(int type)
        {
            return _context.Movies.Where(n=>n.VodFilmType == type && n.IsDeleted == false);
        }

        public DateTime GetUpdateDateTime(int movieType)
        {
            return _context.UpdateLogs
                .Where(n => n.MovieType == movieType).OrderBy(n => n.Id)
                .Select(n => n.UpdateDate)
                .FirstOrDefault();
        }
    }
}
