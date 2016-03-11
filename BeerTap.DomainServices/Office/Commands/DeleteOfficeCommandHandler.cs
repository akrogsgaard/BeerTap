using System;
using System.Threading;
using System.Threading.Tasks;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Office.Commands
{
    public class DeleteOfficeCommandHandler : IAsyncCommandHandler<DeleteOfficeCommand>
    {
        private readonly IOfficeRepository _officeRepository;

        public DeleteOfficeCommandHandler(IOfficeRepository officeRepository)
        {
            if (officeRepository == null) throw new ArgumentNullException(nameof(officeRepository));
            _officeRepository = officeRepository;
        }

        public async Task HandleAsync(DeleteOfficeCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (command == null) throw new ArgumentNullException(nameof(command));

            await _officeRepository.DeleteAsync(command.Id, command.UserId).ConfigureAwait(false);
        }
    }
}
