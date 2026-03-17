using Ai_ShopBot.Application.Features.Orders.Commands.Create;
using Ai_ShopBot.Application.Features.Orders.Queries.GetOrderById;
using Ai_ShopBot.Application.Features.Orders.Queries.GetUserOrdersWithPagination;
using Ai_ShopBot.Application.Features.Products.Queries.GetProductsWithPrompt.GetProductsWithPrompt;
using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using OpenAI;
using OpenAI.Embeddings;
using System.ClientModel;
using System.ComponentModel;
using System.Reflection;

namespace Ai_ShopBot.Application.Extensions
{
    public static class ServicesCollection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration config)
        {

            services
                .AddMapping()
                .AddMediator()
                .AddAiModels(config)
                .AddValidators();

            return services;
        }
        static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(configuration =>
            {
                configuration.RegisterServicesFromAssembly(typeof(ServicesCollection).Assembly);
            });

            return services;
        }

        static IServiceCollection AddMapping(this IServiceCollection services)
        {
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton(config);

            services.AddScoped<IMapper, ServiceMapper>();
            return services;
        }

        static IServiceCollection AddAiModels(this IServiceCollection services, IConfiguration config)
        {
            var openAIOptions = new OpenAIClientOptions()
            {
                Endpoint = new Uri(config["AzureOpenAI:Endpoint"]!)
            };

            var githubClient = new OpenAIClient(new ApiKeyCredential(config["AzureOpenAI:ApiKey"]!), openAIOptions)
                .GetChatClient(config["AzureOpenAI:ChatModel"])
                .AsIChatClient();

            services.AddChatClient(githubClient)
                .UseFunctionInvocation();

            services.AddEmbeddingGenerator(
                new OpenAIClient(
                        new ApiKeyCredential(config["AzureOpenAI:ApiKey"]!),
                        openAIOptions)
                    .GetEmbeddingClient(config["AzureOpenAI:EmbeddingModel"]!)
                    .AsIEmbeddingGenerator());

            services.AddScoped<GetProductsWithPromptPlugin>();

            services.AddKernel()
                .Plugins
                .AddFromType<GetProductsWithPromptPlugin>("Get_products")
                .AddFromType<CreateOrderPlugin>("Create_order")
                .AddFromType<GetOrdersPlugin>("Get_Orders")
                .AddFromType<GetOrderByIdPlugin>("Get_Order_By_Id");

            services.AddSingleton(sp => sp.GetRequiredService<IChatClient>()
            .AsChatCompletionService());

            return services;
        }

        static IServiceCollection AddValidators(this IServiceCollection services)
        {
            return services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
