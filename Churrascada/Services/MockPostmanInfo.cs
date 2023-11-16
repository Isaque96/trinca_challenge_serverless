using System.Configuration;
using Domain.Database;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.Azure.Cosmos;

namespace Churrascada.Services;

public static class MockPostmanInfo
{
    public static async Task CreatePostmanInfo()
    {
        var cosmosConn = new CosmosConn(new CosmosClient(
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
        ));

        var churrasList = new List<Churras>
        {
            new(
                "Comemoração do Natal na Trinca!",
                ChurrasStatus.New,
                new DateTime(2023, 12, 21, 19, 0, 0),
                true,
                "56a2a1f6-375d-4b54-a502-96f9f37db99b"
            ),
            new(
                "Comemoração da Chegada do novo Dev Isaque!",
                ChurrasStatus.Confirmed,
                new DateTime(2023, 11, 22, 18, 0, 0),
                true,
                "491cc249-97d0-4dd4-93a0-84751cae48cd"
            )
        };

        var personList = new List<Person>
        {
            new(
                "Grande Moderador 1",
                true,
                "171f9858-ddb1-4adf-886b-2ea36e0f0644"
            ),
            new(
                "Grande Moderador 2",
                true,
                "3f74e6bd-11b2-4d48-a294-239a7a2ce7d5"
            ),
            new(
                "Isaque Schuwarte Silva",
                false,
                "addd0967-6e16-4328-bab1-eec63bf31968"
            ),
            new(
                "Super Funcionario 1",
                false,
                "e5c7c990-7d75-4445-b5a2-700df354a6a0"
            ),
            new (
                "Super Funcionario 2",
                false,
                "795fc8f2-1473-4f19-b33e-ade1a42ed123"
            )
        };

        var agendaList = new List<Agenda>();

        var agendaRepository = new AgendaRepository(cosmosConn);
        var churrasRepository = new ChurrasRepository(cosmosConn, agendaRepository);
        var personRepository = new PersonRepository(cosmosConn, agendaRepository, churrasRepository);
        
        foreach (var churras in churrasList)
            await churrasRepository.AddAsync(churras);

        foreach (var person in personList)
            await personRepository.AddAsync(person);

        foreach (var agenda in agendaList)
            await agendaRepository.AddAsync(agenda);
    }
}