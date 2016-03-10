using System;
using System.Collections.Generic;
using BeerTap.Transport;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Office.Queries
{
    public class GetOfficeByNameQuery : IQuery<IEnumerable<OfficeDto>>
    {
        private readonly string _name;

        public GetOfficeByNameQuery(string name)
        {
            if (name == null) throw new ArgumentNullException("name");
            _name = name;
        }

        public string Name { get { return _name; } }
    }
}
