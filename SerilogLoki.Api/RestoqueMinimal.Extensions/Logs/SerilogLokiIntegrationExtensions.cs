
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.Grafana.Loki;

namespace RestoqueMinimal.Extensions.Logs
{
    public static class SerilogLokiIntegrationExtensions
    {
        public static Logger ConfigureStructuralLogWithSerilogAndLoki(IConfiguration configuration, WebApplicationBuilder builder)
        {
            var lokiUrl = configuration["ObservabiltyConfiguration:LokiUrl"];

            return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft",LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .Enrich.WithProperty("Application", builder.Environment.ApplicationName)
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthcheck")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthcheck-ui")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("HealthChecksDb")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("HealthChecksUI")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("healthchecks-data-ui")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger/index.html")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger/index.html")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("swagger/v1/swagger.json")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("swagger/v1/swagger.json")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Key.ToString().Contains("healthz-json")))
            .Filter.ByExcluding(c => c.Properties.Any(p => p.Value.ToString().Contains("healthz-json")))
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
