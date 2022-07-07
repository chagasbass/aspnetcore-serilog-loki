using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Worker.Serilog.Loki.Shared.Configurations;

namespace Worker.Serilog.Loki.Extensions.DependencyInjections
{
    public static class OptionsExtensions
    {
        public static IServiceCollection AddOptionsPattern(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BaseConfigurationOptions>(configuration.GetSection(BaseConfigurationOptions.BaseConfig));
            services.Configure<ResilienceConfigurationOptions>(configuration.GetSection(ResilienceConfigurationOptions.ResilienciaConfig));
            services.Configure<WorkerConfigurationOptions>(configuration.GetSection(WorkerConfigurationOptions.WorkerConfig));
            services.Configure<ObservabiltyConfigurationOptions>(configuration.GetSection(ObservabiltyConfigurationOptions.BaseConfig));

            return services;
        }
    }
}
