namespace Churrascada.Database;

public class CosmosConn
{
    private readonly string? _endpoint = Environment.GetEnvironmentVariable("Endpoint");
    private readonly string? _key = Environment.GetEnvironmentVariable("PrimaryKey");
    
    public CosmosConn()
    {
        _cosmosClient = new CosmosClient(_endpoint, _key, new CosmosClientOptions
        {
            ApplicationName = "Churrascada",
            
            AllowBulkExecution = true,
            SerializerOptions = new CosmosSerializationOptions
            {
                Indented = false,
                PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase,
                IgnoreNullValues = false
            }
        });
    }
}