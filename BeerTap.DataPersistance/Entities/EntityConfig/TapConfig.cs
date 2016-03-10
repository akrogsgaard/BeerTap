using System.Data.Entity.ModelConfiguration;

namespace BeerTap.DataPersistance.Entities.EntityConfig
{
    public class TapConfig : EntityTypeConfiguration<TapRecord>
    {
        public TapConfig()
        {
            this.ToTable("Taps");
            this.HasKey(x => new { Id = x.Id });
        }
    }
}
