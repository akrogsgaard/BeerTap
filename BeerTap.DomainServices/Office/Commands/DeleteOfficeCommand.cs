namespace BeerTap.DomainServices.Office.Commands
{
    public class DeleteOfficeCommand
    {
        private readonly int _id;
        private readonly int _userId;

        public DeleteOfficeCommand(int id, int userId)
        {
            _id = id;
            _userId = userId;
        }

        public int Id { get { return _id; } }
        public int UserId { get { return _userId; } }
    }
}
