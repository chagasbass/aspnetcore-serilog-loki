using Microsoft.Extensions.DependencyInjection;

namespace RestoqueMinimal.Extensions.CustomResults
{
    public static class CustomResultsExtensions
    {
        public static IServiceCollection AddApiCustomResults(this IServiceCollection services)
        {
            services.AddSingleton<IApiCustomResults, ApiCustomResults>();
            return services;
        }
    }
}
