using System;
using BeerTap.Services;

namespace BeerTap.ApiServices
{
    public interface IDomainServiceResolver
    {
        IDomainService Resolve(Type requestedServiceType);

        TService Resolve<TService>()
            where TService : IDomainService;
    }
}

namespace BeerTap.Services
{
    /// <summary> 
    /// Represents a specific domain service / repository used in IApiApplicationService implementations 
    /// </summary> 
    public interface IDomainService
    {
    }
}
