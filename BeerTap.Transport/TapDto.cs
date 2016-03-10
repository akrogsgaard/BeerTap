using System;

namespace BeerTap.Transport
{
    public class TapDto
    {
        public int Id { get; set; }
        public int OfficeId { get; set; }
        public int KegId { get; set; }
        public KegDto Keg { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
    }
}