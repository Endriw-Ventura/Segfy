using Microsoft.Extensions.DependencyInjection;
using Segfy.API.Application;
using Segfy.Application.Mappings;
using Segfy.Application.Mappings.Apolices;
using Segfy.Application.Mappings.Sinistros;

namespace Segfy.Application.DependencyInjection
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(
            this IServiceCollection services)
        {
            services.AddAutoMapper(
            cfg => { },
            typeof(SinistroProfile),
            typeof(HistoricoSinistroProfile),
            typeof(ApoliceProfile)
            );

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ApplicationAssembly).Assembly);
            });
            return services;
        }
    }
}
