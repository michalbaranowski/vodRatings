using System;
using System.Collections.Generic;
using vod.Repository.Boundary.Models;

namespace vod.Domain.Services.Utils
{
    public static class Extension
    {
        public static IEnumerable<MovieEntity> FillStoredDate(this IEnumerable<MovieEntity> collection)
        {
            var dateNow = DateTime.Now;
            foreach (var item in collection)
            {
                item.StoredDate = dateNow;
                yield return item;
            }
        }
    }
}
