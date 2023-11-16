using Churrascada.Middlewares;
using Churrascada.Services;
using Microsoft.Extensions.Hosting;

var host = new HostBuilder()
    .ConfigureServices(ChurrascadaServiceCollection.ConfigureServiceCollection)
    .ConfigureFunctionsWorkerDefaults(builder => builder.UseMiddleware<OnErrorMiddleware>())
    .Build();

host.Run();