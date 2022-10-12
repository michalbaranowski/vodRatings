using System;
using System.Collections.Generic;
using vod.Domain.Services.Boundary.Interfaces;

namespace vod.Domain.Services.Utils.HtmlSource.Serialize
{
    public class DisneyPlusSerializer : IDisneyPlusSerializer
    {
        public IEnumerable<string> SerializeUrls(string sitemapHtml)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FilmResult> Serialize(string html)
        {
            throw new NotImplementedException();
        }
    }
}
