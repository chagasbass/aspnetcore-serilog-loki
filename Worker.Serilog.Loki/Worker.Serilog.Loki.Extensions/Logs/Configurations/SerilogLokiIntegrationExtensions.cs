using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace Worker.Serilog.Loki.Extensions.Logs
{
    public static class SerilogLokiIntegrationExtensions
    {
        public static Logger ConfigureStructuralLogWithSerilogAndLoki()
        {
            var lokiUrl = @"http://localhost:3100";
            //https://localhost:7150/

            return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", "Worker.Serilog.Loki")
            //.Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .Destructure.ByTransforming<HttpRequest>(x => new
            {
                x.Method,
                Url = x.Path,
                x.QueryString
            })
            .WriteTo.Console()
            .WriteTo.GrafanaLoki(lokiUrl)
            .CreateLogger();
        }
    }
}
