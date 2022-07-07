using Worker.Serilog.Loki.Shared.Logs;

namespace Worker.Serilog.Loki.Shared.Entities
{
    public static class WorkerLogData
    {
        public static LogData LogData { get; set; } = new LogData();

    }
}
