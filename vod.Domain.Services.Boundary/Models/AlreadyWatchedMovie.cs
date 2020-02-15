using System;
using System.Collections.Generic;
using System.Text;

namespace vod.Domain.Services.Boundary.Models
{
    public class AlreadyWatchedMovie
    {
        public string Title { get; set; }
        public int Id { get; set; }
        public string UserId { get; set; }
        public int MovieId => this.Id;
    }
}
