using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vod.Repository.Boundary.Models;

namespace vod.Domain.Services.Utils
{
    public static class Extension
    {
        public static bool NeedRefresh(this IList<ResultModel> collection)
        {
            return collection.FirstOrDefault()?.RefreshDate < DateTime.Now.AddDays(-1);
        }
    }
}
