using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vod.Repository.Boundary.Models
{
    [Table("UserMovie")]
    public class UserMovieEntity
    {
        [Key]
        public int Id { get; set; }        
        public string UserId { get; set; }
        public int MovieId { get; set; }

        [ForeignKey("UserId")]
        public virtual IdentityUser User { get; set; }
        [ForeignKey("MovieId")]
        public virtual ICollection<MovieEntity> Movies { get; set; }
    }
}
