using System;
using System.Threading;
using System.Threading.Tasks;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Keg.Commands
{
    public class DeleteKegCommandHandler : IAsyncCommandHandler<DeleteKegCommand>
    {
        private readonly IKegRepository _kegRepository;

        public DeleteKegCommandHandler(IKegRepository kegRepository)
        {
            if (kegRepository == null) throw new ArgumentNullException("KegRepository");
            _kegRepository = kegRepository;
        }

        public async Task HandleAsync(DeleteKegCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (command == null) throw new ArgumentNullException("command");

            await _kegRepository.DeleteAsync(command.Id, command.UserId).ConfigureAwait(false);
        }
    }
}
