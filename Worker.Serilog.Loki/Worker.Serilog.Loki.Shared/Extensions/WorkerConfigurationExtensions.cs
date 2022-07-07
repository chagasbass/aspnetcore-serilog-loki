using Microsoft.Extensions.Configuration;
using Worker.Serilog.Loki.Shared.Configurations;

namespace Worker.Serilog.Loki.Shared.Extensions
{
    /// <summary>
    /// Classe de configuração para a troca de parâmetros do worker em tempo de execução
    /// </summary>
    public static class WorkerConfigurationExtensions
    {
        public static void ReloadOptions(this WorkerConfigurationOptions workerConfigurationOptions)
        {
            var runtimeConfiguration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json").Build();

            var value = runtimeConfiguration["WorkerConfiguration:Runtime"];
            workerConfigurationOptions.Runtime = Int32.Parse(value);
        }
    }
}
