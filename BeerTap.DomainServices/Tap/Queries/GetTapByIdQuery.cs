﻿using BeerTap.Transport;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.CQS;

namespace BeerTap.DomainServices.Tap.Queries
{
    public class GetTapByIdQuery : IQuery<Option<TapDto>>
    {
        private readonly int _id;

        public GetTapByIdQuery(int id)
        {
            _id = id;
        }

        public int Id { get { return _id; } }
    }
}
