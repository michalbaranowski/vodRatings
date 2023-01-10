using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vod.Repository.Boundary.Models
{
    [Table("Movie")]
    public class MovieEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal FilmwebRating { get; set; }
        public int FilmwebRatingCount { get; set; }
        public string ImageUrl { get; set; }
        public int Year { get; set; }
        public string ProviderName { get; set; }
        public string Production { get; set; }
        public DateTime StoredDate { get; set; }
        public string FilmwebFilmType { get; set; }
        public int? VodFilmType { get; set; }
        public string FilmDescription { get; set; }
        public DateTime RefreshDate { get; set; }
        public string MovieUrl { get; set; }
        public string Cast { get; set; }
        public bool IsDeleted { get; set; }
        public string OriginalTitle { get; set; }

        [Obsolete("Do usunięcia")]
        [NotMapped]
        public bool IsAlreadyWatched { get; set; }

        public int DurationInMinutes { get; set; }
    }
}
