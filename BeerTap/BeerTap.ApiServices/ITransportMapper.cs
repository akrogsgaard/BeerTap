using System;
using IQ.Platform.Framework.Common;

namespace BeerTap.ApiServices
{
    public interface ITransportMapper<TTransport, TResource, TKey>
        where TResource : IIdentifiable<TKey>
    {
        TResource MapToResource(TTransport transport);
        TTransport MapToTransport(TResource resource, Func<TTransport> transportCreator = null);
    }
}
