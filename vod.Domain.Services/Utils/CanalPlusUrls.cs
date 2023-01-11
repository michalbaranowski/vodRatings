using System;
using System.Collections.Generic;
using System.Text;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Utils
{
    public static class CanalPlusUrls
    {
        private const int _numberOfMoviesToLoad = 100;

        public static readonly string CanalPlusBaseUrl = "https://www.canalplus.com";
        public static readonly string CanalPlusApiUrl = "https://hodor.canalplus.pro/";

        public static readonly string CanalPlusMovieUrl 
            = $"{CanalPlusApiUrl}api/v2/mycanalint/strateContent/08e336458f30c749731b2f778ae7c153/pfs/spyro/contents/";

        public static string GetUrlWithType(MovieTypes type)
        {
            var result = string.Empty;

            switch (type)
            {
                case MovieTypes.Action:
                case MovieTypes.Thriller:
                    result += $"{CanalPlusMovieUrl}ncplus-ouah-filmy-trzymajacewnapieciu.json";
                    break;
                case MovieTypes.Comedy:
                    result += $"{CanalPlusMovieUrl}filmy-na-poprawe-nastroju.json";
                    break;
                case MovieTypes.Cartoons:
                    result += $"{CanalPlusMovieUrl}ncplus-ouah-filmy-dlacalejrodziny.json";
                    break;
                default:
                    break;
            }


            
            var additionalParams = $"?displayNBOLogo=false&discoverMode=true&dsp=detailPage&sdm=show&displayLogo=true&imageRatio=34&titleDisplayMode=none&context_type=edito&context_list_category=Trzymajace+w+napieciu&context_page_title=Theme+-+[OTTTVE]+Filmy&context_list_title=Trzymajace+w+napieciu&context_list_id=ncplus-ouah-filmy-trzymajacewnapieciu&context_list_type=contentRow&context_list_position=4&after=YXJyYXljb25uZWN0aW9uOjEz&maxContentRemaining={_numberOfMoviesToLoad}&get={_numberOfMoviesToLoad}";
            return $"{result}{additionalParams}";
        }
    }
}
