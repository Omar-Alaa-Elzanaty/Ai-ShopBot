using Ai_shopBot.Infrastructure.Services;
using Ai_ShopBot.Croe.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Ai_shopBot.Infrastructure.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services
                .AddServices();

            return services;
        }

        static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IAuthServices, AuthServices>();

            return services;
        }
    }
}
