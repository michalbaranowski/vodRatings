﻿using System;
using System.Collections.Generic;
using vod.Repository.Boundary.Models;

namespace vod.Repository.Boundary
{
    public interface IVodRepositoryBackground
    {
        void RefreshData(IEnumerable<ResultModel> results, int type);
    }
}