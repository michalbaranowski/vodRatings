using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Core.Boundary.Model
{
    public class Result
    {
        public string Title { get; set; }
        public decimal FilmwebRating { get; set; }
        public int FilmwebRatingCount { get; set; }
        public byte[] Image { get; set; }
        public int Year { get; set; }
        public string ProviderName { get; set; }
    }
}
