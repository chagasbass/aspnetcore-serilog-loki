using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestoqueMinimal.Extensions.Shared.Configurations;
using RestoqueTemplateMinimalApi.Shared.Configurations;

namespace RestoqueMinimal.Extensions.Options
{
    public static class OptionsExtensions
    {
        public static IServiceCollection AddBaseConfigurationOptionsPattern(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BaseConfigurationOptions>(configuration.GetSection(BaseConfigurationOptions.BaseConfig));
            services.Configure<ProblemDetailConfigurationOptions>(configuration.GetSection(ProblemDetailConfigurationOptions.BaseConfig));
            services.Configure<HealthchecksConfigurationOptions>(configuration.GetSection(HealthchecksConfigurationOptions.BaseConfig));
            services.Configure<ResilienceConfigurationOptions>(configuration.GetSection(ResilienceConfigurationOptions.ResilienciaConfig));
            services.Configure<ObservabiltyConfigurationOptions>(configuration.GetSection(ObservabiltyConfigurationOptions.BaseConfig));

            return services;
        }
    }
}
