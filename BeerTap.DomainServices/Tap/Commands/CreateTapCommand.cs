using System;

namespace BeerTap.DomainServices.Tap.Commands
{
    public class CreateTapCommand
    {
        private readonly int _officeId;
        private readonly int _createdByUserId;

        public CreateTapCommand(int officeId, int createdByUserId)
        {
            _officeId = officeId;
            _createdByUserId = createdByUserId;
        }

        public int OfficeId { get { return _officeId; } }
        public int CreatedByUserId { get { return _createdByUserId; } }
    }
}
