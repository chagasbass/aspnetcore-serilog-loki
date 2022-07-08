using System.Diagnostics;
using Worker.Serilog.Loki.Extensions.Telemetry;

namespace Worker.Serilog.Loki.Domain.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly IHttpClientFactory _httpClient;
        private readonly ActivitySource _activitySource;

        public WorkerService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
            _activitySource = OpenTelemetryExtensions.CreateActivitySource();
        }

        public async Task CallLokiApiAsync()
        {
            var externalClient = _httpClient.CreateClient();

            var urlConsumo = @"https://localhost:7150/log-information";

            var categoriaResponse = await externalClient.GetAsync(urlConsumo);

            //using var activity = _activitySource.StartActivity("Retorno da Chamada", ActivityKind.Producer);

            if (categoriaResponse.IsSuccessStatusCode)
            {
                var json = await categoriaResponse.Content.ReadAsStringAsync();


                //activity!.SetTag("timestamp", $"{DateTime.Now:HH:mm:ss dd/MM/yyyy}");
                //activity!.SetTag("response.data", json);

                //activity.AddEvent(new("Chamada para minimal api loki", DateTimeExtensions.GetGmtDateTime(DateTime.Now)));
            }
            else
            {
                var json = await categoriaResponse.Content.ReadAsStringAsync();

                //activity!.SetTag("timestamp", $"{DateTime.Now:HH:mm:ss dd/MM/yyyy}");
                //activity!.SetTag("response.data", json);

                //activity.AddEvent(new("Chamada para minimal api loki", DateTimeExtensions.GetGmtDateTime(DateTime.Now)));
            }
        }
    }
}
