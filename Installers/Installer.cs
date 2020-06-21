using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CheerMeApp.Installers
{
    public interface IInstaller
    {
        void InstallServices(IServiceCollection serviceCollection, IConfiguration configuration);
    }
}