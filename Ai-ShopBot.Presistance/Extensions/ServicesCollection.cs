using Ai_ShopBot.Application.Interfaces;
using Ai_ShopBot.Application.Interfaces.Repo;
using Ai_ShopBot.Core.Models;
using Ai_ShopBot.Presistance.Context;
using Ai_ShopBot.Presistance.Repos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using StackExchange.Redis;

namespace Ai_ShopBot.Presistance.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddContext(configuration)
                .AddRedis(configuration)
                .AddMongoDb(configuration)
                .AddCollections();

            return services;
        }
        static IServiceCollection AddContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<ShopDbContext>(options =>
               options.UseLazyLoadingProxies().UseSqlServer(connectionString,
                   builder => builder.MigrationsAssembly(typeof(ShopDbContext).Assembly.FullName)));

            // Identity configuration
            services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<ShopDbContext>()
                    .AddUserManager<UserManager<User>>()
                    .AddRoleManager<RoleManager<IdentityRole>>()
                    .AddSignInManager<SignInManager<User>>()
                    .AddSignInManager()
                    .AddDefaultTokenProviders();

            return services;
        }
        static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration config)
        {
            services
                .AddSingleton<IMongoClient>(_ =>
                new MongoClient(config.GetConnectionString("mongodb")));

            return services;
        }
        static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>(
                ConnectionMultiplexer.Connect(configuration["Redis"]!));

            return services;
        }

        static IServiceCollection AddCollections(this IServiceCollection services)
        {
            services
                .AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IProductRepository, ProductRepository>()
                .AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
