using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Office.Queries
{
    public class GetAllOfficesQueryHandler : IAsyncQueryHandler<GetAllOfficesQuery, IEnumerable<OfficeDto>>
    {
        private readonly IOfficeRepository _officeRepository;

        public GetAllOfficesQueryHandler(IOfficeRepository officeRepository)
        {
            if (officeRepository == null) throw new ArgumentNullException("officeRepository");
            _officeRepository = officeRepository;
        }

        public async Task<IEnumerable<OfficeDto>> HandleAsync(GetAllOfficesQuery query, CancellationToken cancellationToken = new CancellationToken())
        {
            return await _officeRepository.GetAllAsync().ConfigureAwait(false);
        }
    }
}
