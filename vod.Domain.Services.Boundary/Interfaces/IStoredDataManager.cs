﻿using System;
using System.Collections.Generic;
using vod.Domain.Services.Boundary.Models;
using vod.Repository.Boundary;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IStoredDataManager
    {
        IEnumerable<FilmwebResult> UseStorageIfPossible(Func<IEnumerable<FilmwebResult>> func);
    }
}