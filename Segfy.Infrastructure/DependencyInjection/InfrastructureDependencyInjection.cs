using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Segfy.Application.Interfaces;
using Segfy.Application.Interfaces.Apolice;
using Segfy.Application.Interfaces.Sinistro;
using Segfy.Infrastructure.Persistence;
using Segfy.Infrastructure.Persistence.Context;
using Segfy.Infrastructure.Persistence.Repositories;


namespace Segfy.Infrastructure.DependencyInjection
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(
       this IServiceCollection services,
       IConfiguration configuration)
        {
            services.AddDbContext<AppDBContext>(options =>
                options.UseSqlite(
                    configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ISinistroRepository, SinistroRepository>();
            services.AddScoped<IApoliceRepository, ApoliceRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
