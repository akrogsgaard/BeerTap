using System;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Keg.Queries
{
    public class GetKegByIdQueryHandler : IAsyncQueryHandler<GetKegByIdQuery, Option<KegDto>>
    {
        private readonly IKegRepository _kegRepository;

        public GetKegByIdQueryHandler(IKegRepository kegRepository)
        {
            if (kegRepository == null) throw new ArgumentNullException(nameof(kegRepository));
            _kegRepository = kegRepository;
        }

        public async Task<Option<KegDto>> HandleAsync(GetKegByIdQuery query, CancellationToken cancellationToken = new CancellationToken())
        {
            var record = await _kegRepository.GetByIdAsync(query.Id).ConfigureAwait(false);
            return Option.Some(record).NoneIfNull();
        }
    }
}
