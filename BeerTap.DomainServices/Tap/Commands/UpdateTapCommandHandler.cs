using System;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;
using Infrastructure;

namespace BeerTap.DomainServices.Tap.Commands
{
    public class UpdateTapCommandHandler : IAsyncCommandHandler<UpdateTapCommand>
    {
        private readonly ITapRepository _tapRepository;

        public UpdateTapCommandHandler(ITapRepository tapRepository)
        {
            if (tapRepository == null) throw new ArgumentNullException(nameof(tapRepository));
            _tapRepository = tapRepository;
        }

        public async Task HandleAsync(UpdateTapCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var tapDto = new TapDto
            {
                Id = command.Id,
                OfficeId = command.OfficeId,
                KegId = command.KegId,
                KegState = command.KegState,
                UpdatedByUserId = command.UpdatedByUserId,
                UpdatedDateUtc = TimeProvider.Current.UtcNow,
            };

            await _tapRepository.UpdateAsync(tapDto).ConfigureAwait(false);
        }
    }
}
