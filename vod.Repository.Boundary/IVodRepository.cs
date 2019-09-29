using System;
using System.Collections.Generic;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IVodRepository
    {
        void SaveData(IEnumerable<ResultModel> results);
        IEnumerable<ResultModel> GetStoredData();
    }
}
