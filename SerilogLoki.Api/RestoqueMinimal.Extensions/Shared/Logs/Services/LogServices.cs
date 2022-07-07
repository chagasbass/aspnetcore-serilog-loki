using RestoqueMinimal.Extensions.Shared.Logs;
using Serilog;


namespace RestoqueMinimal.Extensions.Shared.Logs.Services
{
    public class LogServices : ILogServices
    {
        public LogData LogData { get; set; }

        private readonly ILogger _logger = Log.ForContext<LogServices>();

        public LogServices()
        {
            LogData = new LogData();
        }

        public void WriteLog()
        {
            var mensagem = "Requisição efetuada";
            _logger.Information("[LogRequisição]:{mensagem}", mensagem);

            if (LogData.RequestData is not null)
                _logger.Information("[RequestData]:{@RequestData}", LogData.RequestData);

            _logger.Information("[Method]:{RequestMethod}", LogData.RequestMethod);
            _logger.Information("[Path]:{RequestUri}", LogData.RequestUri);
            _logger.Information("[RequestTraceId]:{TraceId}", LogData.TraceId);
            _logger.Information("[ResponseData]:{@ResponseData}", LogData.ResponseData);
            _logger.Information("[ResponseStatusCode]:{@ResponseStatusCode}", LogData.ResponseStatusCode);

            LogData.ClearLogData();
        }

        public void WriteLogWhenRaiseExceptions()
        {
            if (LogData is not null && LogData.Exception is not null)
            {
                _logger.Error("[ExceptionType]:{Exception}", LogData.Exception.GetType().Name);
                _logger.Error("[ExceptionMessage]:{Message}", LogData.Exception.Message);
                _logger.Error("[StackTrace]:{StackTrace}", LogData.Exception.StackTrace);

                if (LogData?.Exception?.InnerException is not null)
                {
                    _logger.Error("[InnerException]:{InnerException}", LogData.Exception?.InnerException?.Message);
                }

                LogData.ClearLogExceptionData();
            }

        }

        public void CreateStructuredLog(LogData logData) => LogData = logData;

        public void WriteMessage(string mensagem) => _logger.Information($"{mensagem}");
    }
}
