using System;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Tap.Queries
{
    public class GetTapByIdQueryHandler : IAsyncQueryHandler<GetTapByIdQuery, Option<TapDto>>
    {
        private readonly ITapRepository _tapRepository;

        public GetTapByIdQueryHandler(ITapRepository tapRepository)
        {
            if (tapRepository == null) throw new ArgumentNullException("TapRepository");
            _tapRepository = tapRepository;
        }

        public async Task<Option<TapDto>> HandleAsync(GetTapByIdQuery query, CancellationToken cancellationToken = new CancellationToken())
        {
            var record = await _tapRepository.GetByIdAsync(query.Id).ConfigureAwait(false);
            return Option.Some(record).NoneIfNull();
        }
    }
}
