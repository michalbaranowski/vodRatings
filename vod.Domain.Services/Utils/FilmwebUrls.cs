namespace vod.Domain.Services.Utils
{
    public static class FilmwebUrls
    {
        public static readonly string FilmwebBaseUrl = "https://www.filmweb.pl";
        public static string FilmwebSearchBaseUrl(string title) => $"https://www.filmweb.pl/films/search?q={title.FormatForUrl()}";

        private static string FormatForUrl(this string title)
        {
            return title.Replace(" ", "%20").Replace("?", "");
        }
    }
}
