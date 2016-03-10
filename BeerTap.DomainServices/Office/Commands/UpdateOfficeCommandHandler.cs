using System;
using System.Threading;
using System.Threading.Tasks;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;
using Infrastructure;

namespace BeerTap.DomainServices.Office.Commands
{
    public class UpdateOfficeCommandHandler : IAsyncCommandHandler<UpdateOfficeCommand>
    {
        private readonly IOfficeRepository _officeRepository;

        public UpdateOfficeCommandHandler(IOfficeRepository officeRepository)
        {
            if (officeRepository == null) throw new ArgumentNullException("officeRepository");
            _officeRepository = officeRepository;
        }


        public async Task HandleAsync(UpdateOfficeCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (command == null) throw new ArgumentNullException("command");

            var officeDto = new OfficeDto
                {
                    Id = command.Id,
                    Name = command.Name,
                    CreatedByUserId = command.CreatedByUserId,
                    CreatedDateUtc = TimeProvider.Current.UtcNow,
                    UpdatedByUserId = command.CreatedByUserId,
                    UpdatedDateUtc = TimeProvider.Current.UtcNow,
                };

            await _officeRepository.UpdateAsync(officeDto).ConfigureAwait(false);
        }
    }
}
