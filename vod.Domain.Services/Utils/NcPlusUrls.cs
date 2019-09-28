using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Domain.Services.Utils
{
    public static class NcPlusUrls
    {
        public static readonly string NcPlusBaseUrl = "http://ncplusgo.pl/";
        public static readonly string VodPremieryBaseUrl =
            $"{NcPlusBaseUrl}Collection/Vod/Details?collectionCodename=premiery&ParentCategoryCodename=film&CollectionCategoryCodename=Vod&ViewType=collections&page=1&CategoryCodename=thriller-3";

        public static readonly string VodCplusBaseUrl =
            $"{NcPlusBaseUrl}Collection/CatchUp/Details?collectionCodename=catchup-canal-vod&CollectionCategoryCodename=CatchUp&ViewType=collections&ParentCategoryCodename=film&page=1&CategoryCodename=thriller-3";

        public static readonly string VodHboBaseUrl =
            $"{NcPlusBaseUrl}Collection/Vod/Details?collectionCodename=vod-hbo-go&ParentCategoryCodename=film&CollectionCategoryCodename=Vod&ViewType=collections&page=1&CategoryCodename=thriller-3";
    }
}
