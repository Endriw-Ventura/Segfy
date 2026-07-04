using Microsoft.Extensions.DependencyInjection;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.Interfaces.Sinistro;
using Segfy.Application.Mappings;
using Segfy.Application.Mappings.Apolice;
using Segfy.Application.Mappings.Sinistro;
using Segfy.Application.Services.Apolice;
using Segfy.Application.Services.Sinistro;

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
            services.AddScoped<ISinistroService, SinistroService>();
            services.AddScoped<IApoliceService, ApoliceService>();
            return services;
        }
    }
}
