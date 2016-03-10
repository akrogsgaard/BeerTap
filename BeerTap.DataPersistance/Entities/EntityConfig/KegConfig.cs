using System.Data.Entity.ModelConfiguration;

namespace BeerTap.DataPersistance.Entities.EntityConfig
{
    public class KegConfig : EntityTypeConfiguration<KegRecord>
    {
        public KegConfig()
        {
            this.ToTable("Kegs");
            this.HasKey(x => new { Id = x.Id });
        }
    }
}
