using System;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;
using Infrastructure;

namespace BeerTap.DomainServices.Keg.Commands
{
    public class UpdateKegCommandHandler : IAsyncCommandHandler<UpdateKegCommand>
    {
        private readonly IKegRepository _kegRepository;

        public UpdateKegCommandHandler(IKegRepository kegRepository)
        {
            if (kegRepository == null) throw new ArgumentNullException(nameof(kegRepository));
            _kegRepository = kegRepository;
        }

        public async Task HandleAsync(UpdateKegCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var kegDto = new KegDto
            {
                Id = command.Id,
                TapId = command.TapId,
                BeerName = command.BeerName,
                Capacity = command.Capacity,
                Volume = command.Volume,
                UpdatedByUserId = command.UpdatedByUserId,
                UpdatedDateUtc = TimeProvider.Current.UtcNow,
            };

            await _kegRepository.UpdateAsync(kegDto).ConfigureAwait(false);
        }
    }
}
