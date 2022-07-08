using Worker.Serilog.Loki.Domain.Services;

namespace Worker.Serilog.Loki.Worker.Extensions
{
    public static class WorkerDIExtensions
    {
        public static IServiceCollection SolveWorkerDI(this IServiceCollection services)
        {
            services.AddHttpClient("minimal-api")
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5));

            services.AddTransient<IWorkerService, WorkerService>();
            return services;
        }
    }
}
