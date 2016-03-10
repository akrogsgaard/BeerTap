using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using IQ.Foundation.Logging;

namespace BeerTap.WebApi.Infrastructure.Installers
{
    public class LoggingDependenciesInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<ILogManager>().ImplementedBy<LogManager>());
        }
    }
}