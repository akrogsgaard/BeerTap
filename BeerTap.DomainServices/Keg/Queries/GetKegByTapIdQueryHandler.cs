using System;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Keg.Queries
{
    public class GetKegByTapIdQueryHandler : IAsyncQueryHandler<GetKegByTapIdQuery, Option<KegDto>>
    {
        private readonly IKegRepository _kegRepository;

        public GetKegByTapIdQueryHandler(IKegRepository kegRepository)
        {
            if (kegRepository == null) throw new ArgumentNullException("KegRepository");
            _kegRepository = kegRepository;
        }

        public async Task<Option<KegDto>> HandleAsync(GetKegByTapIdQuery query, CancellationToken cancellationToken = new CancellationToken())
        {
            var record = await _kegRepository.GetByTapIdAsync(query.Id).ConfigureAwait(false);
            return Option.Some(record).NoneIfNull();
        }
    }
}
