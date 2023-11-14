using Churrascada.Services;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureServices(ChurrascadaServiceCollection.ConfigureServiceCollection)
    .ConfigureFunctionsWorkerDefaults()
    .Build();

host.Run();