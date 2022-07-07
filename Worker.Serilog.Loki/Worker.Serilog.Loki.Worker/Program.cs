using Serilog;
using Worker.Serilog.Loki.Extensions.DependencyInjections;
using Worker.Serilog.Loki.Extensions.Logs;
using Worker.Serilog.Loki.Extensions.Telemetry;
using Worker.Serilog.Loki.Worker;
using Worker.Serilog.Loki.Worker.Extensions;

Log.Logger = SerilogLokiIntegrationExtensions.ConfigureStructuralLogWithSerilogAndLoki();

try
{
    Log.Information($"Iniciando o Worker");

    IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        #region configurações do Worker
        var env = hostContext.HostingEnvironment;

        var config = GetConfiguration(args, env);

        //TODO:Inserir as outras extensions criadas
        services.AddOptionsPattern(config)
                .AddApplicationInsightsTelemetry(config)
                .SolveWorkerDI()
                .AddDependencyInjectionExtensions();

        services.Configure<HostOptions>(config.GetSection("HostOptions"));

        #endregion

        services.AddHostedService<RestoqueWorker>();
    })
    .UseSerilog()
    .Build();

    await host.RunAsync();

    return 0;

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminado inexperadamente.");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}



static IConfiguration GetConfiguration(string[] args, IHostEnvironment environment)
{
    return new ConfigurationBuilder()
       .SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
       .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
       .AddUserSecrets<Program>(optional: true, reloadOnChange: true)
       .AddEnvironmentVariables()
       .AddCommandLine(args)
       .Build();
}
