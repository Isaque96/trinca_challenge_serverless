using Domain.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Churrascada.Services;

public static class ChurrascadaServiceCollection
{
    public static void ConfigureServiceCollection(IServiceCollection serviceCollection)
    {
        serviceCollection.AddCosmos<ChurrascadaDbContext>(
            Environment.GetEnvironmentVariable("ConnectionString")!,
            Environment.GetEnvironmentVariable("DbName")!
        );
    }
}