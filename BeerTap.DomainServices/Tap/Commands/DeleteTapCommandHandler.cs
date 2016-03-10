using System;
using System.Threading;
using System.Threading.Tasks;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Tap.Commands
{
    public class DeleteTapCommandHandler : IAsyncCommandHandler<DeleteTapCommand>
    {
        private readonly ITapRepository _tapRepository;

        public DeleteTapCommandHandler(ITapRepository tapRepository)
        {
            if (tapRepository == null) throw new ArgumentNullException("TapRepository");
            _tapRepository = tapRepository;
        }

        public async Task HandleAsync(DeleteTapCommand command, CancellationToken cancellationToken = new CancellationToken())
        {
            if (command == null) throw new ArgumentNullException("command");

            await _tapRepository.DeleteAsync(command.Id, command.UserId).ConfigureAwait(false);
        }
    }
}
