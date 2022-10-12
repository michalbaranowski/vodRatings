using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Domain.Services.Boundary.Models
{
    public class ApiProviderResult : FilmResultWithMovieType
    {
        public string Url { get; set; }
    }
}
