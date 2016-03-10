using System;

namespace BeerTap.DomainServices.Tap.Commands
{
    public class UpdateTapCommand
    {
        private readonly int _id;
        private readonly int _officeId;
        private readonly int _kegId;
        private readonly string _kegState;
        private readonly int _updatedByUserId;

        public UpdateTapCommand(int id, int officeId, int kegId, string kegState, int updatedByUserId)
        {
            _id = id;
            _officeId = officeId;
            _kegId = kegId;
            _kegState = kegState;
            _updatedByUserId = updatedByUserId;
        }

        public int Id { get { return _id; } }
        public int OfficeId { get { return _officeId; } }
        public int KegId { get { return _kegId; } }
        public string KegState { get { return _kegState; } }
        public int UpdatedByUserId { get { return _updatedByUserId; } }
    }
}
