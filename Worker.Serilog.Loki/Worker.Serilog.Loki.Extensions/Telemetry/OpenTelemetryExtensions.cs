using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Worker.Serilog.Loki.Extensions.Telemetry
{
    public static class OpenTelemetryExtensions
    {
        public static string Local { get; }
        public static string Kernel { get; }
        public static string Framework { get; }
        public static string ServiceName { get; }
        public static string ServiceVersion { get; }

        static OpenTelemetryExtensions()
        {
            Local = Environment.MachineName;
            Kernel = Environment.OSVersion.VersionString;
            Framework = RuntimeInformation.FrameworkDescription;
            ServiceName = "Worker.Serilog.Loki";
            ServiceVersion = typeof(OpenTelemetryExtensions).Assembly.GetName().Version!.ToString();
        }

        public static ActivitySource CreateActivitySource() =>
            new ActivitySource(ServiceName, ServiceVersion);
    }
}
//982274168
//986061891