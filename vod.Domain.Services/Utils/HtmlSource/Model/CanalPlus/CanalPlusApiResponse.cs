using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Domain.Services.Utils.HtmlSource.Model.CanalPlus
{
    public class CanalPlusApiResponse
    {
        public CanalPlusContentItem[] Contents { get; set; }
    }

    public class CanalPlusContentItem
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string URLImage { get; set; }
        public CanalPlusContentItemOnClick OnClick { get; set; }
    }

    public class CanalPlusContentItemOnClick
    {
        public string Path { get; set; }
    }
}
