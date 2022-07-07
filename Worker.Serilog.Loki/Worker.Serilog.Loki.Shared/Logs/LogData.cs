using System.Text.Json;
using Worker.Serilog.Loki.Shared.Helpers;

namespace Worker.Serilog.Loki.Shared.Logs
{
    /// <summary>
    /// Representa o log estruturado.Pode ser alterado de acordo com a implementação
    /// </summary>
    public class LogData
    {
        public DateTime Timestamp { get; private set; }
        public string CommandInformation { get; private set; }
        public string Handler { get; private set; }
        public string RequestInformation { get; private set; }
        public string ResponseInformation { get; private set; }
        public string TraceId { get; private set; }
        public Exception Exception { get; private set; }
        public string LogMessage { get; private set; }
        public bool HasLog { get; private set; }

        public LogData()
        {
            Timestamp = DateTime.UtcNow.GetGmtDateTime();
        }

        public void AddDataLog(LogData logData)
        {
            Timestamp = logData.Timestamp;
            CommandInformation = logData.CommandInformation;
            Handler = logData.Handler;
            RequestInformation = logData.RequestInformation;
            TraceId = logData.TraceId;
            Exception = logData.Exception;
            LogMessage = logData.LogMessage;
            HasLog = logData.HasLog;
        }

        public bool ShouldProcessLog() => HasLog;

        public LogData HasLogInformation(bool existeLog)
        {
            HasLog = existeLog;
            return this;
        }

        public LogData AddLogTimestamp()
        {
            Timestamp = DateTime.UtcNow.GetGmtDateTime();

            return this;
        }

        public LogData AddLogMessage(string message)
        {
            if (!string.IsNullOrEmpty(LogMessage))
                return this;

            LogMessage = message;
            return this;
        }

        public LogData AddCommandInformation(object commandInformation)
        {
            CommandInformation = JsonSerializer.Serialize(commandInformation);
            return this;
        }

        public LogData AddHandleName(string handler)
        {
            Handler = handler;
            return this;
        }

        public LogData AddRequestInformation(object requestInformation)
        {
            var jsonRequest = JsonSerializer.Serialize(requestInformation);
            RequestInformation = $"{RequestInformation} - {jsonRequest}";
            return this;
        }

        public LogData AddResponseInformation(object responseInformation)
        {
            ResponseInformation = JsonSerializer.Serialize(responseInformation);

            return this;
        }

        public LogData AddTraceIndendifier()
        {
            TraceId = Guid.NewGuid().ToString();
            return this;
        }

        public LogData AddException(Exception exception)
        {
            Exception = exception;
            return this;
        }

        public LogData ClearLogs()
        {
            Timestamp = DateTime.UtcNow.GetGmtDateTime();
            CommandInformation = string.Empty;
            Handler = string.Empty;
            RequestInformation = string.Empty;
            TraceId = string.Empty;
            Exception = default;
            HasLog = false;
            LogMessage = string.Empty;

            return this;
        }
    }
}
