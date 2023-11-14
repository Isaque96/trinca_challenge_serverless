using Microsoft.Azure.Cosmos;

namespace Domain.Database;

public class CosmosConn : IDisposable
{
    private readonly CosmosClient _cosmos;
    private readonly Microsoft.Azure.Cosmos.Database _database;
    private Dictionary<string, Container> _containers;
    
    public CosmosConn(CosmosClient cosmosClient)
    {
        _cosmos = cosmosClient;
        _database = _cosmos.CreateDatabaseIfNotExistsAsync(Environment.GetEnvironmentVariable("DbName")).Result;
        VerifyContainers();
    }

    private void VerifyContainers()
    {
        var containers = new[] { "Agendas", "Churras", "Persons" };
        _containers = containers
            .Select(
                id => new KeyValuePair<string, Container>(
                    id,
                    _database.CreateContainerIfNotExistsAsync(id, "/id").Result
                )
            )
            .ToDictionary(k => k.Key, v => v.Value);
    }

    public Container GetContainer(string containerName)
        => _containers[containerName];
    
    public void Dispose()
    {
        _cosmos.Dispose();
    }
}