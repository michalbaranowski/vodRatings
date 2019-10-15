using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace vod.Repository.Boundary.Models
{
    [Table("Result")]
    public class ResultModel
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
    }
}
