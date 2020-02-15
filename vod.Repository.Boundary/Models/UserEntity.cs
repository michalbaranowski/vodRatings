using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace vod.Repository.Boundary.Models
{
    [Table("AspNetUsers")]
    public class UserEntity : IdentityUser
    {
        public int? UserMovieId { get; set; }
        [ForeignKey("UserMovieId")]
        public virtual UserMovieEntity UserMovie { get; set; }
    }
}
