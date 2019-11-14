using System.Collections.Generic;
using vod.Domain.Services.Boundary.Interfaces.Enums;

namespace vod.Domain.Services.Boundary.Models
{
    public class Movie
    {
        public string Title { get; set; }
        public byte[] Image { get; set; }
        public string MoreInfoUrl { get; set; }
        public string ProviderName { get; set; }
        public MovieTypes MovieType { get; set; }
        public string OriginalTitle { get; set; }
        public List<string> Directors { get; set; }
    }
}
