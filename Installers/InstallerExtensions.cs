using System;
using System.Linq;
using CheerMeApp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CheerMeApp.Installers
{
    public static class InstallerExtensions
    {
        public static void InstallServicesInAssembly(this IServiceCollection serviceCollection,
            IConfiguration configuration)
        {
            var installers = typeof(Startup).Assembly.ExportedTypes.Where(type =>
                    typeof(IInstaller).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                .Select(Activator.CreateInstance).Cast<IInstaller>().ToList();
            installers.ForEach(installer => installer.InstallServices(serviceCollection, configuration));
        }
    }
}