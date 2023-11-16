using System.Configuration;
using Domain.Database;
using Domain.Repositories;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace Churrascada.Services;

public static class ChurrascadaServiceCollection
{
    public static void ConfigureServiceCollection(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton(new CosmosConn(
            new CosmosClient(
                Environment.GetEnvironmentVariable("Endpoint"),
                Environment.GetEnvironmentVariable("PrimaryKey"),
                new CosmosClientOptions
                {
                    SerializerOptions = new CosmosSerializationOptions
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    },
                    ApplicationName = "Churrascada"
                }
            )
        ));

        serviceCollection.AddTransient<IChurrasRepository, ChurrasRepository>();
        serviceCollection.AddTransient<IAgendaRepository, AgendaRepository>();
        serviceCollection.AddTransient<IPersonRepository, PersonRepository>();
        
        // TODO: Remove Mocked Tests
        MockPostmanInfo.CreatePostmanInfo().Wait();
    }
}