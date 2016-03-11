using System;
using System.Collections.Generic;
using System.Reflection;
using BeerTap.ApiServices;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using IQ.Platform.Framework.Common;
using IQ.Platform.Framework.Common.Mapping;

namespace BeerTap.WebApi.Infrastructure.Installers
{
    public class ResourceMapperInstaller : IWindsorInstaller
    {
        readonly IEnumerable<Assembly> _assemblies;

        public ResourceMapperInstaller(IEnumerable<Assembly> assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException(nameof(assemblies));

            _assemblies = assemblies;
        }

        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            _assemblies.Apply(a => container.Register(Classes.FromAssembly(a).BasedOn(typeof(IMapper<,>)).WithServiceAllInterfaces()));
            _assemblies.Apply(a => container.Register(Classes.FromAssembly(a).BasedOn(typeof(ITransportMapper<,,>)).WithServiceAllInterfaces()));
        }
    }
}
