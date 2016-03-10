using System.Data.Entity.ModelConfiguration;

namespace BeerTap.DataPersistance.Entities.EntityConfig
{
    public class OfficeConfig : EntityTypeConfiguration<OfficeRecord>
    {
        public OfficeConfig()
        {
            this.ToTable("Offices");
            this.HasKey(x => new { Id = x.Id });
        }
    }
}
