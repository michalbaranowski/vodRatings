using System.IO;

namespace vod.Domain.Services.Tests.Resources
{
    public static class HtmlResources
    {
        private const string BASEPATH = "../../../Resources/Htmls/";
        public static string CanalPlusThrillersResultHtml() => ReadFromFile($"{BASEPATH}canalPlusThrillersResult.html");
        public static string CanalPlusConcrteMovieResultHtml() => ReadFromFile($"{BASEPATH}canalPlusConcreteMovieResult.html");
        public static string FilmwebResultHtml() => ReadFromFile($"{BASEPATH}filmwebResult.html");

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
