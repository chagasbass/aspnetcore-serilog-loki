using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Worker.Serilog.Loki.Extensions.Resiliences
{
    public static class ResilienceExtensions
    {
        public static IServiceCollection AddApiResiliencesPatterns(this IServiceCollection services, IConfiguration configuration)
        {
            var quantidadeDeRetentativas = Int32.Parse(configuration["ResilienceConfiguration:QuantidadeDeRetentativas"]);
            var nomeCliente = configuration["ResilenceConfiguration:NomeCliente"];

            services.AddHttpClient(nomeCliente)
                    .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                    .AddPolicyHandler(ResiliencePolicies.GetApiRetryPolicy(quantidadeDeRetentativas));

            return services;
        }
    }
}
