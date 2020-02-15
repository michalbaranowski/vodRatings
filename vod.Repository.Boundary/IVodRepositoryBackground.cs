﻿using System.Collections.Generic;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IVodRepositoryBackground
    {
        void RefreshData(IEnumerable<MovieEntity> results, int type);
        MovieEntity ResultByTitle(string movieTitle);
        IList<MovieEntity> GetResultsOfType(int type);
        void RemoveMovies(IEnumerable<MovieEntity> moviesToRemove);
        void AddMovies(IEnumerable<MovieEntity> moviesToAdd);
    }
}
