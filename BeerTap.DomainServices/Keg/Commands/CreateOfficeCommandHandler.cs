using System;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;
using Infrastructure;

namespace BeerTap.DomainServices.Keg.Commands
{
    public interface ICreateKegCommandHandler : IAsyncCommandHandler<CreateKegCommand>
    {
        Task<int> HandleCustomAsync(CreateKegCommand command, CancellationToken cancellationToken = new CancellationToken());
    }

    public class CreateKegCommandHandler : ICreateKegCommandHandler
    {
        private readonly IKegRepository _kegRepository;

        public CreateKegCommandHandler(IKegRepository kegRepository)
        {
            if (kegRepository == null) throw new ArgumentNullException(nameof(kegRepository));
            _kegRepository = kegRepository;
        }

        public async Task HandleAsync(CreateKegCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<int> HandleCustomAsync(CreateKegCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var kegDto = new KegDto
            {
                TapId = command.TapId,
                BeerName = command.BeerName,
                Capacity = command.Capacity,
                Volume = command.Volume,
                CreatedByUserId = command.CreatedByUserId,
                CreatedDateUtc = TimeProvider.Current.UtcNow,
                UpdatedByUserId = command.CreatedByUserId,
                UpdatedDateUtc = TimeProvider.Current.UtcNow,
            };

            return await _kegRepository.SaveNewAsync(kegDto).ConfigureAwait(false);
        }
    }
}
