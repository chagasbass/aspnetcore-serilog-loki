using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Worker.Serilog.Loki.Extensions.Telemetry
{
    public static class TelemetryExtensions
    {
        public static IServiceCollection AddApplicationInsightsTelemetry(this IServiceCollection services, IConfiguration configuration)
        {
            var instrumentationKey = configuration["ApplicationInsights:InstrumentationKey"];

            services.AddApplicationInsightsTelemetryWorkerService(new ApplicationInsightsServiceOptions
            {
                InstrumentationKey = instrumentationKey
            });

            return services;
        }
    }
}
