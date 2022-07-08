using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Worker.Serilog.Loki.Extensions.Telemetry
{
    public static class DistributedTracingExtensions
    {
        public static IServiceCollection AddTelemetryTracing(this IServiceCollection services)
        {
            var agentHost = "localhost";
            var agentPort = 6831;

            services.AddOpenTelemetryTracing(traceProvider =>
            {
                traceProvider
                    .AddSource(OpenTelemetryExtensions.ServiceName)
                    .AddConsoleExporter()
                    .SetResourceBuilder(
                        ResourceBuilder.CreateDefault()
                            .AddService(serviceName: OpenTelemetryExtensions.ServiceName,
                                serviceVersion: OpenTelemetryExtensions.ServiceVersion))
                    .AddAspNetCoreInstrumentation()
                    .AddJaegerExporter(exporter =>
                    {
                        exporter.AgentHost = agentHost;
                        exporter.AgentPort = agentPort;
                    });
            });

            return services;
        }
    }
}
