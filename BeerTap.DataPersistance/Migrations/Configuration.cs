using System.Collections.Generic;
using BeerTap.DataPersistance.Entities;
using Infrastructure;

namespace BeerTap.DataPersistance.Migrations
{
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<BeerTap.DataPersistance.BeerTapContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(BeerTap.DataPersistance.BeerTapContext context)
        {
            var offices = new List<OfficeRecord> 
                {
                    new OfficeRecord() {Id = 1, Name = "Regina", CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new OfficeRecord() {Id = 2, Name = "Vancouver", CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new OfficeRecord() {Id = 3, Name = "Winnipeg", CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new OfficeRecord() {Id = 4, Name = "Charlotte", CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new OfficeRecord() {Id = 5, Name = "Manila", CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                };

            offices.ForEach(x => context.Offices.AddOrUpdate(y => y.Id, x));

            var taps = new List<TapRecord> 
                {
                    new TapRecord() {Id = 1, OfficeId = 1, KegId = 0, CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new TapRecord() {Id = 2, OfficeId = 1, KegId = 0, CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new TapRecord() {Id = 3, OfficeId = 2, KegId = 0, CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new TapRecord() {Id = 4, OfficeId = 2, KegId = 0, CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new TapRecord() {Id = 5, OfficeId = 3, KegId = 0, CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new TapRecord() {Id = 6, OfficeId = 3, KegId = 0, CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new TapRecord() {Id = 7, OfficeId = 4, KegId = 0, CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new TapRecord() {Id = 8, OfficeId = 4, KegId = 0, CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new TapRecord() {Id = 9, OfficeId = 5, KegId = 0, CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                    new TapRecord() {Id = 10, OfficeId = 5, KegId = 0, CreatedByUserId = 1, CreatedDateUtc = TimeProvider.Current.UtcNow, UpdatedByUserId = 1, UpdatedDateUtc = TimeProvider.Current.UtcNow},
                };

            taps.ForEach(x => context.Taps.AddOrUpdate(y => y.Id, x));
        }
    }
}
