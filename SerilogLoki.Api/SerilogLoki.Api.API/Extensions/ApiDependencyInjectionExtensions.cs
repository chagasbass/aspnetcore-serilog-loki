namespace SerilogLoki.Api.API.Extensions
{
    public static class ApiDependencyInjectionExtensions
    {
        /// <summary>
        /// Adicionar as dependencias criadas e usadas na aplicação
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            return services;
        }
    }
}