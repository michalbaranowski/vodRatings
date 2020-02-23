using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vod.Repository.Boundary.Models
{
    [Table("BlackListedMovie")]
    public class BlackListedMovieEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
