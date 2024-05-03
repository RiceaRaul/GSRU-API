using GSRU_DataAccessLayer.Implementations;
using GSRU_DataAccessLayer.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace GSRU_DataAccessLayer
{
    public static class DataAccessLayerConfiguration
    {
        public static void RegisterDependencies(IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
