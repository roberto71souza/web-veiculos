using Microsoft.Extensions.DependencyInjection;
using WebVeiculos.Core.Helper;
using WebVeiculos.Core.Services.Contratos;
using WebVeiculos.Core.Services.Implementacao;
using WebVeiculos.Models.Repositories;
using WebVeiculos.Models.Repositories.Contratos;
using WebVeiculos.Models.Repositories.Implementacao;

namespace WebVeiculos.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped<ConexaoDbDapper>();
            services.AddScoped<IVeiculoRepository, VeiculoRepository>();
            services.AddScoped<IVeiculoService, VeiculoService>();
            services.AddScoped<Util>();
            services.AddAutoMapper(typeof(AutoMapperProfile));

            return services;
        }
    }
}
