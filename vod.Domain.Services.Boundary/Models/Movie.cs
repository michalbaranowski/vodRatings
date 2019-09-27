using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Domain.Services.Boundary.Models
{
    public class Movie
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public int Year { get; set; }
        public decimal Rating { get; set; }
    }
}
