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

        public void RefreshData(IEnumerable<MovieEntity> results, int type)
        {
            using (var ctx = new AppDbContext(_opt))
            {
                ctx.Movies.RemoveRange(ctx.Movies.Where(n=>n.VodFilmType == type));
                ctx.SaveChanges();

                var resultsList = results.ToList();
                resultsList = AddStoredAndRefreshDate(resultsList);

                ctx.Movies.AddRange(resultsList);
                ctx.SaveChanges();
            }
        }

        private List<MovieEntity> AddStoredAndRefreshDate(List<MovieEntity> results)
        {
            foreach (var result in results)
            {
                result.RefreshDate = DateTime.Now;

                if (result.StoredDate == DateTime.MinValue)
                    result.StoredDate = DateTime.Now;
            }

            return results;
        }

        public MovieEntity ResultByTitle(string movieTitle)
        {
            using (var ctx = new AppDbContext(_opt))
            {
                return ctx.Movies.FirstOrDefault(n => n.Title == movieTitle);
            }
        }

        public IList<MovieEntity> GetResultsOfType(int type)
        {
            using (var ctx = new AppDbContext(_opt))
            {
                return ctx.Movies.Where(n => n.VodFilmType == type).ToList();
            }
        }

        public void MarkAsDeleted(IEnumerable<MovieEntity> moviesToRemove)
        {
            using (var ctx = new AppDbContext(_opt))
            {
                foreach (var item in moviesToRemove)
                {
                    item.IsDeleted = true;
                }

                ctx.UpdateRange(moviesToRemove);
                ctx.SaveChanges();
            }
        }

        public void AddMovies(IEnumerable<MovieEntity> moviesToAdd)
        {
            using (var ctx = new AppDbContext(_opt))
            {
                ctx.AddRange(moviesToAdd);
                ctx.SaveChanges();
            }
        }

        public void LogUpdate(int movieType)
        {
            using (var ctx = new AppDbContext(_opt))
            {
                ctx.UpdateLogs.Add(new UpdateLogEntity() { UpdateDate = DateTime.Now, MovieType = movieType });
                ctx.SaveChanges();
            }
        }

        public DateTime GetUpdateDateTime(int movieType)
        {
            using (var ctx = new AppDbContext(_opt))
            {
                return ctx.UpdateLogs
                    .Where(n => n.MovieType == movieType).OrderBy(n => n.Id)
                    .Select(n => n.UpdateDate)
                    .FirstOrDefault();
            }
        }

        public void AddBlackListedMovies(IEnumerable<BlackListedMovieEntity> moviesToBlackList)
        {
            using (var ctx = new AppDbContext(_opt))
            {
                ctx.BlackListedMovies.AddRange(moviesToBlackList);
                ctx.SaveChanges();
            }
        }

        public IList<BlackListedMovieEntity> GetBlackListedMovies()
        {
            using (var ctx = new AppDbContext(_opt))
            {
                return ctx.BlackListedMovies.ToList();
            }
        }
    }
}
