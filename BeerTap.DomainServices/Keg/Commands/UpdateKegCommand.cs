using System;

namespace BeerTap.DomainServices.Keg.Commands
{
    public class UpdateKegCommand
    {
        private readonly int _id;
        private readonly int _tapId;
        private readonly string _state;
        private readonly string _beerName;
        private readonly int _capacity;
        private readonly int _volume;
        private readonly int _updatedByUserId;

        public UpdateKegCommand(int id, int tapId, string state, string beerName, int capacity, int volume, int updatedByUserId)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            if (beerName == null) throw new ArgumentNullException("beerName");
            _id = id;
            _tapId = tapId;
            _state = state;
            _beerName = beerName;
            _capacity = capacity;
            _volume = volume;
            _updatedByUserId = updatedByUserId;
        }

        public int Id { get { return _id; } }
        public int TapId { get { return _tapId; } }

        public string State { get { return _state; } }
        public string BeerName { get { return _beerName; } }
        public int Capacity { get { return _capacity; } }
        public int Volume { get { return _volume; } }
        public int UpdatedByUserId { get { return _updatedByUserId; } }
    }
}
