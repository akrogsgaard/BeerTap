using BeerTap.Transport;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Office.Queries
{
    public class GetOfficeByIdQuery : IQuery<Option<OfficeDto>>
    {
        private readonly int _id;

        public GetOfficeByIdQuery(int id)
        {
            _id = id;
        }

        public int Id { get { return _id; } }
    }
}
