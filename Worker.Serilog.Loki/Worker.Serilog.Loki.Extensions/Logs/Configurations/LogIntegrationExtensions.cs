using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting.Compact;
using Worker.Serilog.Loki.Shared.Logs;
using Worker.Serilog.Loki.Shared.Logs.Services;

namespace Worker.Serilog.Loki.Extensions.Logs.Configurations
{
    public static class LogIntegrationsExtensions
    {
        public static Logger ConfigureLog()
        {
            var logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Error)
                .MinimumLevel.Override("System", LogEventLevel.Error)
                .Enrich.FromLogContext()
                .WriteTo.Console(new CompactJsonFormatter());

            return logger.CreateLogger();
        }

        public static IServiceCollection SolveLogServiceDependencies(this IServiceCollection services)
        {
            services.AddSingleton<ILogServices, LogServices>();
            services.AddSingleton<LogData>();

            return services;
        }
    }
}
