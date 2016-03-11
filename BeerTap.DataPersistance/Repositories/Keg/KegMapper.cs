using System;
using BeerTap.DataPersistance.Entities;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.Mapping;

namespace BeerTap.DataPersistance.Repositories.Keg
{
    public class KegMapper : IMapper<KegRecord, KegDto>,
            IMapper<KegDto, KegRecord>
    {
        public KegDto Map(KegRecord source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new KegDto
                {
                    Id = source.Id,
                    TapId = source.TapId,
                    BeerName = source.BeerName,
                    Capacity = source.Capacity,
                    Volume = source.Volume,
                    CreatedByUserId = source.CreatedByUserId,
                    CreatedDateUtc = source.CreatedDateUtc,
                    UpdatedByUserId = source.UpdatedByUserId,
                    UpdatedDateUtc = source.UpdatedDateUtc,
                };
        }

        public KegRecord Map(KegDto source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new KegRecord
                {
                    Id = source.Id,
                    TapId = source.TapId,
                    BeerName = source.BeerName,
                    Capacity = source.Capacity,
                    Volume = source.Volume,
                    CreatedByUserId = source.CreatedByUserId,
                    CreatedDateUtc = source.CreatedDateUtc,
                    UpdatedByUserId = source.UpdatedByUserId,
                    UpdatedDateUtc = source.UpdatedDateUtc,
                };
        }
    }
}
