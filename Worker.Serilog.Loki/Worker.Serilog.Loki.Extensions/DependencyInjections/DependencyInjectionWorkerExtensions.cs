using Microsoft.Extensions.DependencyInjection;
using Worker.Serilog.Loki.Extensions.Logs.Configurations;

namespace Worker.Serilog.Loki.Extensions.DependencyInjections
{
    public static class DependencyInjectionWorkerExtensions
    {
        public static IServiceCollection AddDependencyInjectionExtensions(this IServiceCollection services)
        {
            services.SolveLogServiceDependencies();

            return services;
        }
    }
}
