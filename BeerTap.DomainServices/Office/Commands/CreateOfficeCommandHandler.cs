using System;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;
using Infrastructure;

namespace BeerTap.DomainServices.Office.Commands
{
    public interface ICreateOfficeCommandHandler : IAsyncCommandHandler<CreateOfficeCommand>
    {
        Task<int> HandleCustomAsync(CreateOfficeCommand command, CancellationToken cancellationToken = new CancellationToken());
    }

    public class CreateOfficeCommandHandler : ICreateOfficeCommandHandler
    {
        private readonly IOfficeRepository _officeRepository;

        public CreateOfficeCommandHandler(IOfficeRepository officeRepository)
        {
            if (officeRepository == null) throw new ArgumentNullException(nameof(officeRepository));
            _officeRepository = officeRepository;
        }

        public async Task HandleAsync(CreateOfficeCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public async Task<int> HandleCustomAsync(CreateOfficeCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            var officeDto = new OfficeDto
            {
                Name = command.Name,
                CreatedByUserId = command.CreatedByUserId,
                CreatedDateUtc = TimeProvider.Current.UtcNow,
                UpdatedByUserId = command.CreatedByUserId,
                UpdatedDateUtc = TimeProvider.Current.UtcNow,
            };

            return await _officeRepository.SaveNewAsync(officeDto).ConfigureAwait(false);
        }
    }
}
