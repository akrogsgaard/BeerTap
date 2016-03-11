using System;
using BeerTap.DataPersistance.Entities;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.Mapping;

namespace BeerTap.DataPersistance.Repositories.Office
{
    public class OfficeMapper : IMapper<OfficeRecord, OfficeDto>,
            IMapper<OfficeDto, OfficeRecord>
    {
        public OfficeDto Map(OfficeRecord source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new OfficeDto
                {
                    Id = source.Id,
                    Name = source.Name,
                    //Taps = source.Taps,
                    CreatedByUserId = source.CreatedByUserId,
                    CreatedDateUtc = source.CreatedDateUtc,
                    UpdatedByUserId = source.UpdatedByUserId,
                    UpdatedDateUtc = source.UpdatedDateUtc,
                };
        }

        public OfficeRecord Map(OfficeDto source)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return new OfficeRecord
                {
                    Id = source.Id,
                    Name = source.Name,
                    //Taps = source.Taps,
                    CreatedByUserId = source.CreatedByUserId,
                    CreatedDateUtc = source.CreatedDateUtc,
                    UpdatedByUserId = source.UpdatedByUserId,
                    UpdatedDateUtc = source.UpdatedDateUtc,
                };
        }
    }
}
