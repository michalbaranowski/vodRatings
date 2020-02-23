using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace vod.Repository.Boundary.Models
{
    [Table("AlreadyWatchedMovie")]
    public class AlreadyWatchedModel
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Username { get; set; }
    }
}
