using System;
using System.Reflection;
using Castle.Core;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using IQ.Platform.Framework.Logging;
using BeerTap.ApiServices;
using BeerTap.Services;

namespace BeerTap.WebApi.Infrastructure.Installers
{

    /// <summary>
    /// Registers domain specific services using a service resolver if provided, otherwise register all services from provided assembly.
    /// </summary>
    public class DomainServicesInstaller : IWindsorInstaller
    {
        readonly IDomainServiceResolver _customDomainServiceResolver;
        readonly Assembly _apiDomainServicesAssembly;
        readonly Assembly _apiDomainServiceInterfacesAssembly;

        public DomainServicesInstaller(IDomainServiceResolver customDomainServiceResolver, Assembly apiDomainServicesAssembly,
                                       Assembly apiDomainServiceInterfacesAssembly)
        {
            if (apiDomainServicesAssembly == null)
                throw new ArgumentNullException(nameof(apiDomainServicesAssembly));
            if (apiDomainServiceInterfacesAssembly == null)
                throw new ArgumentNullException(nameof(apiDomainServiceInterfacesAssembly));

            _customDomainServiceResolver = customDomainServiceResolver;
            _apiDomainServicesAssembly = apiDomainServicesAssembly;
            _apiDomainServiceInterfacesAssembly = apiDomainServiceInterfacesAssembly;
        }


        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container
                .Register(Classes.FromAssembly(_apiDomainServicesAssembly).BasedOn<IDomainService>()
                 .Configure(
                                    cr =>
                                    {
                                        var x =
                                            cr.Interceptors(InterceptorReference.ForKey(LoggingConstants.DomainServiceLogger)).Anywhere;
                                    })
                .WithServiceFromInterface());
        }
    }
}
