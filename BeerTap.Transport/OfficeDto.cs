using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeerTap.Transport
{
    public class OfficeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<TapDto> Taps { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public int UpdatedByUserId { get; set; }
        public DateTime UpdatedDateUtc { get; set; }
    }
}
