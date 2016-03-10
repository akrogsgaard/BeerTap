using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerTap.DataPersistance.Entities
{
    public class TapRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public int KegId { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
    }
}
