using System;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;
using Infrastructure;

namespace BeerTap.DomainServices.Tap.Commands
{
    public interface ICreateTapCommandHandler : IAsyncCommandHandler<CreateTapCommand>
    {
        Task<int> HandleCustomAsync(CreateTapCommand command, CancellationToken cancellationToken = new CancellationToken());
    }

    public class CreateTapCommandHandler : ICreateTapCommandHandler
    {
        private readonly ITapRepository _tapRepository;

        public CreateTapCommandHandler(ITapRepository tapRepository)
        {
            if (tapRepository == null) throw new ArgumentNullException(nameof(tapRepository));
            _tapRepository = tapRepository;
        }

        public async Task HandleAsync(CreateTapCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<int> HandleCustomAsync(CreateTapCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var tapDto = new TapDto
            {
                OfficeId = command.OfficeId,
                CreatedByUserId = command.CreatedByUserId,
                CreatedDateUtc = TimeProvider.Current.UtcNow,
                UpdatedByUserId = command.CreatedByUserId,
                UpdatedDateUtc = TimeProvider.Current.UtcNow,
            };

            return await _tapRepository.SaveNewAsync(tapDto).ConfigureAwait(false);
        }
    }
}
