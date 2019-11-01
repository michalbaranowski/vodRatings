using System;
using System.IO;

namespace vod.Domain.Services.Tests.Resources
{
    public static class HtmlResources
    {
        private const string BASEPATH = "../../../Resources/Htmls/";
        public static string CanalPlusThrillersResultHtml() => ReadFromFile($"{BASEPATH}canalPlusThrillersResult.html");
        public static string CanalPlusComediesResultHtml() => ReadFromFile($"{BASEPATH}canalPlusComedyResult.html");
        public static string HboComediesResultHtml() => ReadFromFile($"{BASEPATH}hboKomedie.html");
        public static string NcPremieresResultHtml() => ReadFromFile($"{BASEPATH}premieryKomedie.html");
        public static string CanalPlusConcrteMovieResultHtml() => ReadFromFile($"{BASEPATH}canalPlusConcreteMovieResult.html");
        public static string FilmwebResultHtml() => ReadFromFile($"{BASEPATH}filmwebResult.html");
        public static string FilmWebSearchResultHtml() => ReadFromFile($"{BASEPATH}filmWebSearchResult.html");
        public static string FilmWebSearchResult2Html() => ReadFromFile($"{BASEPATH}filmWebSearchResult2.html");

        private static string ReadFromFile(string path)
        {
            var result = string.Empty;
            FileStream fileStream = new FileStream(path, FileMode.Open);
            using (StreamReader reader = new StreamReader(fileStream))
            {
                result = reader.ReadToEnd();
            }

            return result;
        }
    }
}
