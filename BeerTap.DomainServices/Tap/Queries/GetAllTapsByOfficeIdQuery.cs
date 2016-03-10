using System.Collections.Generic;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Tap.Queries
{
    public class GetAllTapsByOfficeIdQuery : IQuery<IEnumerable<TapDto>>
    {
        private readonly int _officeId;

        public GetAllTapsByOfficeIdQuery(int officeId)
        {
            _officeId = officeId;
        }

        public int OfficeId { get { return _officeId; } }
    }
}
