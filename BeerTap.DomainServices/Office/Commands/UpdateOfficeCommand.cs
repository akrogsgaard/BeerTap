using System;

namespace BeerTap.DomainServices.Office.Commands
{
    public class UpdateOfficeCommand
    {
        private readonly int _id;
        private readonly string _name;
        private readonly int _createdByUserId;

        public UpdateOfficeCommand(int id, string name, int createdByUserId)
        {
            if (name == null) throw new ArgumentNullException("name");
            _id = id;
            _name = name;
            _createdByUserId = createdByUserId;
        }

        public int Id { get { return _id; } }
        public string Name { get { return _name; } }
        public int CreatedByUserId { get { return _createdByUserId; } }
    }
}
