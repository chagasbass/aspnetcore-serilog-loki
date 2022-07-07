using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RestoqueMinimal.Extensions.Shared.Logs;
using RestoqueMinimal.Extensions.Shared.Logs.Services;
using RestoqueMinimal.Extensions.Shared.Notifications;
using Serilog;
using Serilog.Core;
using Serilog.Events;

namespace RestoqueMinimal.Extensions.Logs
{
    public static class LogIntegrationsExtensions
    {
        public static Logger ConfigureStructuralLogWithSerilog()
        {
            return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Information)
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Error)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
            .MinimumLevel.Override("System", LogEventLevel.Error)
            .Enrich.FromLogContext()
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
            .CreateLogger();
        }

        public static IServiceCollection AddLogServiceDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ILogServices, LogServices>();
            services.AddSingleton<LogData>();

            return services;
        }

        public static IServiceCollection AddNotificationControl(this IServiceCollection services)
        {
            services.AddSingleton<INotificationServices, NotificationServices>();
            return services;
        }
    }
}
