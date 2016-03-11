using System;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Office.Queries
{
    public class GetOfficeByIdQueryHandler : IAsyncQueryHandler<GetOfficeByIdQuery, Option<OfficeDto>>
    {
        private readonly IOfficeRepository _officeRepository;

        public GetOfficeByIdQueryHandler(IOfficeRepository officeRepository)
        {
            if (officeRepository == null) throw new ArgumentNullException(nameof(officeRepository));
            _officeRepository = officeRepository;
        }

        public async Task<Option<OfficeDto>> HandleAsync(GetOfficeByIdQuery query, CancellationToken cancellationToken = new CancellationToken())
        {
            var record = await _officeRepository.GetByIdAsync(query.Id).ConfigureAwait(false);
            return Option.Some(record).NoneIfNull();
        }
    }
}
