using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace vod.Repository.Boundary.Models
{
    [Table("UpdateLog")]
    public class UpdateLogEntity
    {
        public int Id { get; set; }
        public DateTime UpdateDate { get; set; }
        public int MovieType { get; set; }
    }
}
