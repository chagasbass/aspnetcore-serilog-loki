namespace RestoqueMinimal.Extensions.Shared.Configurations
{
    public  class ObservabiltyConfigurationOptions
    {
        public const string BaseConfig = "ObservabiltyConfiguration";

        public string? LokiUrl { get; set; }

        public ObservabiltyConfigurationOptions() { }
    }
}
