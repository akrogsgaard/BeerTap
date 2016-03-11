using System;

namespace BeerTap.DomainServices.Keg.Commands
{
    public class CreateKegCommand
    {
        private readonly int _tapId;
        private readonly string _beerName;
        private readonly int _capacity;
        private readonly int _volume;
        private readonly int _createdByUserId;

        public CreateKegCommand(int tapId, string beerName, int capacity, int volume, int createdByUserId)
        {
            if (beerName == null) throw new ArgumentNullException(nameof(beerName));
            _tapId = tapId;
            _beerName = beerName;
            _capacity = capacity;
            _volume = volume;
            _createdByUserId = createdByUserId;
        }

        public int TapId { get { return _tapId; } }
        public string BeerName { get { return _beerName; } }
        public int Capacity { get { return _capacity; } }
        public int Volume { get { return _volume; } }
        public int CreatedByUserId { get { return _createdByUserId; } }
    }
}
