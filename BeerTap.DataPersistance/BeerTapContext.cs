using System.Data.Entity;
using BeerTap.DataPersistance.Entities;
using BeerTap.DataPersistance.Entities.EntityConfig;

namespace BeerTap.DataPersistance
{
    public class BeerTapContext : DbContext
    {
        private readonly int _userId;
        private const string ConnectionString = "name=DefaultConnectionString";

        public BeerTapContext() :
            base(ConnectionString)
        {
        }

        public BeerTapContext(string connectionString, int userId) : 
            base(connectionString)
        {
            _userId = userId;
        }

        public BeerTapContext(string connectionString) :
            base(connectionString)
	    {
	    }

        public DbSet<KegRecord> Kegs { get; set; }
        public DbSet<OfficeRecord> Offices { get; set; }
        public DbSet<TapRecord> Taps { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new KegConfig());
            modelBuilder.Configurations.Add(new OfficeConfig());
            modelBuilder.Configurations.Add(new TapConfig());

            base.OnModelCreating(modelBuilder);
        }
    }
}
