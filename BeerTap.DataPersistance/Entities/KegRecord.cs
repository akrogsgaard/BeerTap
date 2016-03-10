using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerTap.DataPersistance.Entities
{
    public class KegRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int TapId { get; set; }
        public string BeerName { get; set; }
        public int Capacity { get; set; }
        public int Volume { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
    }
}
