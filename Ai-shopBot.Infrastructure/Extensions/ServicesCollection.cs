using Ai_shopBot.Infrastructure.Services;
using Ai_ShopBot.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ai_shopBot.Infrastructure.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddServices();

            return services;
        }

        static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IAuthServices, AuthServices>()
                .AddScoped<IAiChatAssistantServices, AiChatAssistantSerivces>();

            return services;
        }
    }
}
