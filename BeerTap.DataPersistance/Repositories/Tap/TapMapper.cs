using System;
using BeerTap.DataPersistance.Entities;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.Mapping;

namespace BeerTap.DataPersistance.Repositories.Tap
{
    public class TapMapper : IMapper<TapRecord, TapDto>,
            IMapper<TapDto, TapRecord>
    {
        public TapDto Map(TapRecord source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new TapDto
                {
                    Id = source.Id,
                    OfficeId = source.OfficeId,
                    KegId = source.KegId,
                    //Keg = source.Keg,
                    CreatedByUserId = source.CreatedByUserId,
                    CreatedDateUtc = source.CreatedDateUtc,
                    UpdatedByUserId = source.UpdatedByUserId,
                    UpdatedDateUtc = source.UpdatedDateUtc,
                };
        }

        public TapRecord Map(TapDto source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new TapRecord
                {
                    Id = source.Id,
                    OfficeId = source.OfficeId,
                    KegId = source.KegId,
                    //Keg = source.Keg,
                    CreatedByUserId = source.CreatedByUserId,
                    CreatedDateUtc = source.CreatedDateUtc,
                    UpdatedByUserId = source.UpdatedByUserId,
                    UpdatedDateUtc = source.UpdatedDateUtc,
                };
        }
    }
}
