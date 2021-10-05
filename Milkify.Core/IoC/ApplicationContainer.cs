using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Milkify.Core.Repository;

namespace Milkify.Core.IoC
{
    public static class ApplicationContainer
    {
        public static void ConfigureApplicationContainer(this IServiceCollection services,
            IHostingEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            if (hostingEnvironment != null)
            {
                if (string.IsNullOrEmpty(hostingEnvironment.WebRootPath))
                {
                    hostingEnvironment.WebRootPath = Path.Combine(hostingEnvironment.ContentRootPath, "wwwroot");
                }
            }

            services.AddSingleton(services);
        }


        public static void ConfigureAutoRegisterServices(this IServiceCollection services,
            IEnumerable<Assembly> assemblies)
        {
            var definedConcreteTypes = assemblies.GetDefinedConcreteTypes();

            services.AutoRegisterDependencyClassesFromTypes(definedConcreteTypes);
        }


        public static void AutoRegisterDependencyClassesFromTypes(this IServiceCollection services,
            Type[] definedConcreteTypes)
        {
            var dependencyTypes = definedConcreteTypes
                .Where(
                    type =>
                        type.GetInterfaces()
                            .Any(intf => intf.Name.Equals(nameof(IDependency)) ||
                                         intf.Name.Equals(nameof(ISingletonDependency))));

            RegisterDependencyServices<IDependency, ISingletonDependency>(services, dependencyTypes);

            var selfRegisterType = definedConcreteTypes
                .Where(type => !type.IsGenericType)
                .Where(
                    type =>
                        type.GetInterfaces()
                            .Any(
                                intf =>
                                    intf.Name.Equals(nameof(ISingletonSelfRegisterDependency)) ||
                                    intf.Name.Equals(nameof(ISelfRegisterDependency))));

            services.RegisterDependencyClasses<ISelfRegisterDependency, ISingletonSelfRegisterDependency>(
                selfRegisterType);
        }

        public static void RegisterDependencyClasses<TDependency, TSingleonDependency>(this IServiceCollection service,
            IEnumerable<Type> selfRegisterType)
        {
            foreach (var dependencyType in selfRegisterType)
            {
                var interfaces = dependencyType.GetInterfaces();

                if (interfaces.Any(x => x.Name.Equals(typeof(TSingleonDependency).Name)))
                {
                    service.AddSingleton(dependencyType);
                }
                else
                {
                    service.AddScoped(dependencyType);
                }
            }
        }

        public static void RegisterDependencyServices<TDependency, TSingleonDependency>(this IServiceCollection service,
            IEnumerable<Type> dependencyTypes)
        {
            foreach (var dependencyType in dependencyTypes)
            {
                var interfaces = dependencyType.GetInterfaces()
                    .Where(
                        x =>
                            !x.Name.Equals(typeof(TDependency).Name) &&
                            !x.Name.Equals(typeof(TSingleonDependency).Name))
                    .ToArray();

                // Get only direct parent interfaces
                interfaces = interfaces.Except(interfaces.SelectMany(t => t.GetInterfaces())).ToArray();

                foreach (var intf in interfaces)
                {
                    if (intf.IsGenericType && dependencyType.IsGenericType)
                    {
                        if (interfaces.Any(x =>
                            x.GetInterfaces().Any(itf => itf.Name.Equals(typeof(TSingleonDependency).Name))))
                        {
                            service.AddSingleton(intf.GetGenericTypeDefinition(),
                                dependencyType.GetGenericTypeDefinition());
                            service.AddSingleton(dependencyType.GetGenericTypeDefinition());
                        }
                        else
                        {
                            service.AddScoped(intf.GetGenericTypeDefinition(),
                                dependencyType.GetGenericTypeDefinition());
                            service.AddScoped(dependencyType.GetGenericTypeDefinition());
                        }
                    }
                    else
                    {
                        if (interfaces.Any(x =>
                            x.GetInterfaces().Any(itf => itf.Name.Equals(typeof(TSingleonDependency).Name))))
                        {
                            service.AddSingleton(intf, dependencyType);
                            service.AddSingleton(dependencyType);
                        }
                        else
                        {
                            service.AddScoped(intf, dependencyType);
                            service.AddScoped(dependencyType);
                        }
                    }
                }
            }
        }
    }
}
