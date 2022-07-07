using Serilog;
using Serilog.Context;
using System.Text;
using Worker.Serilog.Loki.Shared.Logs;

namespace Worker.Serilog.Loki.Shared.Logs.Services
{
    public class LogServices : ILogServices
    {
        public LogData LogData { get; set; }

        private readonly ILogger _logger = Log.ForContext<LogServices>();

        public LogServices()
        {
            LogData = new LogData();
        }

        public void WriteLog(string projectName)
        {
            if (LogData.ShouldProcessLog())
            {
                var logInformation = new StringBuilder();

                using (LogContext.PushProperty("Log da operação", projectName))
                {
                    logInformation.AppendLine($"[Message]:{LogData.LogMessage}");
                    logInformation.AppendLine($"[Trace Id]:{LogData.TraceId}");
                    logInformation.AppendLine($"[Log Timestamp]:{LogData.Timestamp}");
                    logInformation.AppendLine($"[Handler]:{LogData.Handler}");
                    logInformation.AppendLine($"[Command Information]:{LogData.CommandInformation}");
                    logInformation.AppendLine($"[Request Information]:{LogData.RequestInformation}");
                    logInformation.AppendLine($"[Response Information]:{LogData.ResponseInformation}");

                    _logger.Information(logInformation.ToString());
                }

                LogData.ClearLogs();

            }
        }

        public void WriteLogWhenRaiseExceptions(string projectName)
        {
            using (LogContext.PushProperty("Log da operação", projectName))
            {

                if (LogData is not null)
                {
                    using (LogContext.PushProperty("Log da operação", projectName))
                    {
                        var logInformation = new StringBuilder();

                        logInformation.AppendLine($"[Exception]: {LogData.Exception.GetType().Name}");
                        logInformation.AppendLine($"[Exception Message]: {LogData.Exception.Message}");
                        logInformation.AppendLine($"[Exception StackTrace]: {LogData.Exception.StackTrace}");

                        if (LogData.Exception.InnerException is not null)
                        {
                            logInformation.AppendLine($"[InnerException]: {LogData.Exception?.InnerException?.Message}");
                        }

                        _logger.Error(logInformation.ToString());


                        LogData.ClearLogs();
                    }
                }
            }
        }

        public void CreateStructuredLog(LogData logData) => LogData = logData;

        public void WriteMessage(string mensagem) => _logger.Information($"{mensagem}");
    }
}
