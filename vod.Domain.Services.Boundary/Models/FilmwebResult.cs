using NodaTime;
using System;
using System.Collections.Generic;

namespace vod.Domain.Services.Boundary.Models
{
    public class FilmwebResult
    {
        public int Id { get; set; }
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
        public string FilmDescription { get; set; }
        public string FilmwebTitle { get; set; }
        public string MovieUrl { get; set; }
        public List<string> Cast { get; set; }
        public bool IsAlreadyWatched { get; set; }
        public string OriginalTitle { get; set; }
        public Duration Duration { get; set; }
    }
}
