﻿using System;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Utils
{
    public static class NcPlusUrls
    {
        public static readonly string NcPlusGoUrl = "http://ncplusgo.pl/";
        public static readonly string VodPremieryBaseUrl =
            $"{NcPlusGoUrl}Collection/Vod/Details?collectionCodename=premiery&ParentCategoryCodename=film&CollectionCategoryCodename=Vod&ViewType=collections&page=1";

        public static readonly string VodCplusBaseUrl =
            $"{NcPlusGoUrl}Collection/CatchUp/Details?collectionCodename=catchup-canal-vod&CollectionCategoryCodename=CatchUp&ViewType=collections&ParentCategoryCodename=film&page=1";

        public static readonly string VodHboBaseUrl =
            $"{NcPlusGoUrl}Collection/Vod/Details?collectionCodename=vod-hbo-go&ParentCategoryCodename=film&CollectionCategoryCodename=Vod&ViewType=collections&page=1";

        public static string GetUrlWithType(this string baseUrl, MovieTypes type)
        {
            switch (type)
            {
                case MovieTypes.Thriller:
                    return $"{baseUrl}&CategoryCodename=thriller-3";
                case MovieTypes.Action:
                    return $"{baseUrl}&CategoryCodename=akcja-2";
                case MovieTypes.Comedy:
                    return $"{baseUrl}&CategoryCodename=komedia-2";
                default:
                    throw new NotImplementedException("Brak implementacji dla podanego typu!");
            }
        }
    }
}
