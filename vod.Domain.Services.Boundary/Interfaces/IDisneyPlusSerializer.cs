﻿using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Domain.Services.Boundary.Interfaces
{
    public interface IDisneyPlusSerializer
    {
        IEnumerable<string> SerializeUrls(string sitemapHtml);
        FilmResultWithMovieType Serialize(string html);
    }
}
