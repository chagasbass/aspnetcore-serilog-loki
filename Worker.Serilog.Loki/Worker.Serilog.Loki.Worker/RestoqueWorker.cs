using Microsoft.ApplicationInsights;
using Microsoft.Extensions.Options;
using NCrontab;
using Worker.Serilog.Loki.Domain.Services;
using Worker.Serilog.Loki.Shared.Configurations;
using Worker.Serilog.Loki.Shared.Extensions;
using Worker.Serilog.Loki.Shared.Logs.Services;

namespace Worker.Serilog.Loki.Worker
{
    public class RestoqueWorker : BackgroundService
    {
        private CrontabSchedule? _schedule;

        private readonly BaseConfigurationOptions _baseConfigurationOptions;
        private readonly WorkerConfigurationOptions _workerConfigOptions;
        private readonly ILogServices _logService;
        private readonly TelemetryClient _telemetryClient;
        private readonly IWorkerService _workerService;

        public RestoqueWorker(IOptionsMonitor<WorkerConfigurationOptions> workerOptions,
                                 IOptionsMonitor<BaseConfigurationOptions> baseOptions,
                                 ILogServices logService,
                                 TelemetryClient telemetryClient, IWorkerService workerService)
        {
            _baseConfigurationOptions = baseOptions.CurrentValue;
            _workerConfigOptions = workerOptions.CurrentValue;
            _logService = logService;
            _telemetryClient = telemetryClient;
            _workerService = workerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {

                    await ProcessAsync();

                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logService.LogData.AddException(ex);
                    _logService.WriteLogWhenRaiseExceptions(_baseConfigurationOptions.NomeProjeto);
                    _logService.WriteMessage("Restart do Serviço Iniciado...");

                    _workerConfigOptions.ReloadOptions();

                    _telemetryClient.Flush();
                    await Task.Delay(_workerConfigOptions.Runtime, stoppingToken);
                }
            }
        }

        private async Task ProcessAsync()
        {
            _logService.WriteMessage("Iniciando Processamento...");

            await _workerService.CallLokiApiAsync();

            _logService.WriteLog(_baseConfigurationOptions.NomeProjeto);
            _logService.WriteMessage("Fim do ciclo de processamento...");
        }
    }
}