using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BeerTap.DataPersistance.Entities
{
    public class OfficeRecord
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDateUtc { get; set; }

        [ForeignKey("OfficeId")]
        public virtual ICollection<TapRecord> Taps { get; set; }
    }
}
