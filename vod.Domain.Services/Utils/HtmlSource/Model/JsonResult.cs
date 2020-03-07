using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Domain.Services.Utils.HtmlSource.Model
{
    public class NetflixJsonResult
    {
        public int Count { get; set; }
        public List<NetflixJsonResultItems> Items { get; set; }
    }
}
