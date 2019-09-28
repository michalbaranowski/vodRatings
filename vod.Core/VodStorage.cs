using System;
using System.Collections.Generic;
using vod.Core.Boundary.Model;

namespace vod.Core
{
    public static class VodStorage
    {
        public static IEnumerable<Result> StoredResults { get; private set; }
        public static DateTime StorageDate { get; private set; }

        public static void Store(IEnumerable<Result> results)
        {
            StoredResults = results;
            StorageDate = DateTime.Now;
        }
    }
}
