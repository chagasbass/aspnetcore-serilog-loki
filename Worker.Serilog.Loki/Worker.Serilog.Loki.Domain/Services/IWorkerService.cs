namespace Worker.Serilog.Loki.Domain.Services
{
    public interface IWorkerService
    {
        Task CallLokiApiAsync();
    }
}
