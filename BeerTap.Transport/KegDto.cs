using System;

namespace BeerTap.Transport
{
    public class KegDto
    {
        public int Id { get; set; }
        public int TapId  { get; set; }
        public string BeerName { get; set; }
        public int Capacity { get; set; }
        public int Volume { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
    }
}