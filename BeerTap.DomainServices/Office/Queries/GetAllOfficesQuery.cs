using System;
using System.Collections.Generic;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Office.Queries
{
    public class GetAllOfficesQuery : IQuery<IEnumerable<OfficeDto>>
    {
    }
}
