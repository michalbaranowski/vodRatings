namespace vod.Domain.Services.Utils
{
    public static class FilmwebUrls
    {
        public static readonly string FilmwebBaseUrl = "https://www.filmweb.pl";
        public static string FilmwebSearchBaseUrl(string originalTitle) => $"https://www.filmweb.pl/films/search?q={originalTitle.Replace(" ", "%20")}";
    }
}
