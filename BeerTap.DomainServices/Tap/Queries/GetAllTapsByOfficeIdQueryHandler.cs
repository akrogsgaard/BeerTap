using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Tap.Queries
{
    public class GetAllTapsByOfficeIdQueryHandler : IAsyncQueryHandler<GetAllTapsByOfficeIdQuery, IEnumerable<TapDto>>
    {
        private readonly ITapRepository _officeRepository;

        public GetAllTapsByOfficeIdQueryHandler(ITapRepository officeRepository)
        {
            if (officeRepository == null) throw new ArgumentNullException(nameof(officeRepository));
            _officeRepository = officeRepository;
        }

        public async Task<IEnumerable<TapDto>> HandleAsync(GetAllTapsByOfficeIdQuery query, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _officeRepository.GetAllTapsByOfficeIdAsync(query.OfficeId).ConfigureAwait(false);
        }
    }
}
