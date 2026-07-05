using Microsoft.Extensions.DependencyInjection;
using Segfy.Application.Mappings;
using Segfy.Application.Mappings.Apolice;
using Segfy.Application.Mappings.Sinistro;
using Segfy.Application.UseCases.Apolices.Create;
using Segfy.Application.UseCases.Apolices.GetAll;
using Segfy.Application.UseCases.Apolices.GetApoliceById;
using Segfy.Application.UseCases.Apolices.GetApoliceWithSinistros;
using Segfy.Application.UseCases.Apolices.UpdateApoliceStatus;
using Segfy.Application.UseCases.Sinistros.CreateSinistro;
using Segfy.Application.UseCases.Sinistros.GetAllSinistros;
using Segfy.Application.UseCases.Sinistros.GetHistoricoSinistro;
using Segfy.Application.UseCases.Sinistros.GetSinistroById;
using Segfy.Application.UseCases.Sinistros.UpdateSinistroStatus;

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

            services.AddScoped<IGetSinistroByIdUseCase, GetSinistroByIdUseCase>();
            services.AddScoped<IGetAllSinistrosUseCase, GetAllSinistrosUseCase>();
            services.AddScoped<IGetHistoricoSinistroUseCase, GetHistoricoSinistroUseCase>();
            services.AddScoped<ICreateSinistroUseCase, CreateSinistroUseCase>();
            services.AddScoped<IUpdateSinistroStatusUseCase, UpdateSinistroStatusUseCase>();

            services.AddScoped<IGetApoliceByIdUseCase, GetApoliceByIdUseCase>();
            services.AddScoped<IGetAllApoliciesUseCase, GetAllApoliciesUseCase>();
            services.AddScoped<IGetApoliceWithSinistrosUseCase, GetApoliceWithSinistrosUseCase>();
            services.AddScoped<ICreateApoliceUseCase, CreateApoliceUseCase>();
            services.AddScoped<IUpdateApoliceStatusUseCase, UpdateApoliceStatusUseCase>();

            return services;
        }
    }
}
