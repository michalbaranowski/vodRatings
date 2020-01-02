using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using vod.Core.Boundary.Model;
using vod.Domain.Services.Boundary.Models;

namespace vod.Core.Extension
{
    public static class ResultConvertExtension
    {
        public static IEnumerable<Result> AddNewFlagIfNeeded(this IList<Result> result)
        {
            if (result.Any() == false) return result;

            var max = result.Max(n => n.StoredDate);
            var min = result.Min(n => n.StoredDate);

            if (max == min)
                return result;

            foreach (var res in result)
                if (res.StoredDate.Date == max.Date)
                    res.IsNew = true;

            return result;
        }
    }
}
