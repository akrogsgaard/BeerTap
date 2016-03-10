namespace BeerTap.DomainServices.Keg.Commands
{
    public class DeleteKegCommand
    {
        private readonly int _id;
        private readonly int _userId;

        public DeleteKegCommand(int id, int userId)
        {
            _id = id;
            _userId = userId;
        }

        public int Id { get { return _id; } }
        public int UserId { get { return _userId; } }
    }
}
