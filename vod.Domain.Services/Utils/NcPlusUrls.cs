using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Domain.Services.Utils
{
    public static class NcPlusUrls
    {
        public static readonly string VodHboBaseUrl =
            "http://ncplusgo.pl/Collection/Index?CollectionCategoryCodename=Vod&ViewType=collections&ParentCategoryCodename=film&page=1&CategoryCodename=thriller-3";

        public static readonly string VodCplusBaseUrl =
            "https://ncplusgo.pl/Collection/CatchUp/Details?collectionCodename=catchup-canal-vod&CollectionCategoryCodename=CatchUp&ViewType=collections&ParentCategoryCodename=film&page=1&CategoryCodename=thriller-3";
    }
}
