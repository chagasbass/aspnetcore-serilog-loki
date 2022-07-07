using Microsoft.Extensions.DependencyInjection;

namespace RestoqueMinimal.Extensions.Middlewares
{
    public static class MiddlewareExtensions
    {
        public static IServiceCollection AddGlobalExceptionHandlerMiddleware(this IServiceCollection services)
        {
            services.AddTransient<GlobalExceptionHandlerMiddleware>();
            return services;
        }
    }
}
