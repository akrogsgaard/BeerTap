using System;

namespace BeerTap.DomainServices.Office.Commands
{
    public class CreateOfficeCommand
    {
        private readonly string _name;
        private readonly int _createdByUserId;

        public CreateOfficeCommand(string name, int createdByUserId)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            _name = name;
            _createdByUserId = createdByUserId;
        }

        public string Name { get { return _name; } }
        public int CreatedByUserId { get { return _createdByUserId; } }
    }
}
