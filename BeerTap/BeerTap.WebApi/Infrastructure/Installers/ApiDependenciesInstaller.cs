using BeerTap.ApiServices.RequestContext;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BeerTap.WebApi.Infrastructure.Installers
{
    public class ApiDependenciesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IExtractDataFromARequestContext>().ImplementedBy<RequestContextExtractor>());
        }
    }
}