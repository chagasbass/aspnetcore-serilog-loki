namespace Worker.Serilog.Loki.Shared.Configurations
{
    public class WorkerConfigurationOptions
    {
        public const string WorkerConfig = "WorkerConfiguration";
        public string HorarioDeExecucao { get; set; }
        public int Runtime { get; set; }

        public WorkerConfigurationOptions() { }
    }
}
