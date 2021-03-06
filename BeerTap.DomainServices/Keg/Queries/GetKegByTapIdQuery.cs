﻿using BeerTap.Transport;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Keg.Queries
{
    public class GetKegByTapIdQuery : IQuery<Option<KegDto>>
    {
        private readonly int _id;

        public GetKegByTapIdQuery(int id)
        {
            _id = id;
        }

        public int Id { get { return _id; } }
    }
}
