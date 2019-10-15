using System;

namespace vod.Domain.Services.Boundary.Models
{
    public class FilmwebResult
    {
        public string Title { get; set; }
        public decimal FilmwebRating { get; set; }
        public int FilmwebRatingCount { get; set; }
        public string ImageUrl { get; set; }
        public int Year { get; set; }
        public string ProviderName { get; set; }
        public string FilmwebFilmType { get; set; }
        public int VodFilmType { get; set; }
        public string Production { get; set; }
        public DateTime StoredDate { get; set; }
    }
}
