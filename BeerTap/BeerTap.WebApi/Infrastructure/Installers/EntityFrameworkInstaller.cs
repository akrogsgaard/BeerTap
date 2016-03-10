using BeerTap.DataPersistance;
using BeerTap.DataPersistance.Repositories.Keg;
using BeerTap.DataPersistance.Repositories.Office;
using BeerTap.DataPersistance.Repositories.Tap;
using BeerTap.DataPersistance.Version;
using BeerTap.DomainServices.Keg;
using BeerTap.DomainServices.Office;
using BeerTap.DomainServices.Office.Commands;
using BeerTap.DomainServices.Office.Queries;
using BeerTap.DomainServices.Tap;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using IQ.Platform.EntityFrameworkEx.Exceptions;
using IQ.Platform.Framework.Common.CQS;
using FormatDatabaseExceptions = BeerTap.DataPersistance.Exceptions.FormatDatabaseExceptions;

namespace BeerTap.WebApi.Infrastructure.Installers
{
    public class EntityFrameworkInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            RegisterEntitiyFrameworkComponents(container);
            RegisterCommands(container);
            RegisterQueries(container);
            RegisterRepositories(container);
        }

        static void RegisterEntitiyFrameworkComponents(IWindsorContainer container)
        {
            container.Register(Component.For<IFormatDatabaseExceptions>().ImplementedBy<FormatDatabaseExceptions>());
            container.Register(Component.For<IDbContextFactory<BeerTapContext>>().ImplementedBy<BeerTapContextFactory>());
        }

        private static void RegisterCommands(IWindsorContainer container)
        {
            container.Register(Classes.FromAssemblyContaining<CreateOfficeCommandHandler>().BasedOn(typeof(IAsyncCommandHandler<>)).WithServiceAllInterfaces());
        }

        static void RegisterQueries(IWindsorContainer container)
        {
            container.Register(Classes.FromAssemblyContaining<GetDbVersionQueryHandler>().BasedOn(typeof(IAsyncQueryHandler<,>)).WithServiceAllInterfaces());
            container.Register(Classes.FromAssemblyContaining<GetOfficeByIdQueryHandler>().BasedOn(typeof(IAsyncQueryHandler<,>)).WithServiceAllInterfaces());
        }

        static void RegisterRepositories(IWindsorContainer container)
        {
            container.Register(Component.For<IOfficeRepository>().ImplementedBy<OfficeRepository>());
            container.Register(Component.For<ITapRepository>().ImplementedBy<TapRepository>());
            container.Register(Component.For<IKegRepository>().ImplementedBy<KegRepository>());
        }
    }
}