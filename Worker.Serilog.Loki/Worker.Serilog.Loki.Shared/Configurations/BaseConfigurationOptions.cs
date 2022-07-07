namespace Worker.Serilog.Loki.Shared.Configurations
{
    public class BaseConfigurationOptions
    {
        public const string BaseConfig = "BaseConfiguration";

        public string NomeProjeto { get; set; }
        public string StringConexaoBancoDeDados { get; set; }

        public BaseConfigurationOptions() { }

    }
}
