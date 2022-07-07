using Polly;
using Polly.Extensions.Http;
using System.Net;

namespace Worker.Serilog.Loki.Extensions.Resiliences
{
    public static class ResiliencePolicies
    {
        public static IAsyncPolicy<HttpResponseMessage> GetApiRetryPolicy(int quantidadeDeRetentativas)
        {
            var quantidadeTotalDeRetentativas = quantidadeDeRetentativas;

            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode != HttpStatusCode.OK)
                .RetryAsync(quantidadeDeRetentativas, onRetry: (message, numeroDeRetentativas) =>
              {
                  if (quantidadeTotalDeRetentativas == numeroDeRetentativas)
                  {
                  //TODO:Rotina para guardar os logs dentro do tratamento de resiliencia
                  //WorkerLogData.LogData.AddLogTimestamp()
                  //                     .InsertLogMessage("Retentativas de chamadas externas foram excedidas.")
                  //                     .InserirCodigoDeStatusDaResposta((int)message.Result.StatusCode)
                  //                     .InserirVerboDaRequisicao(message.Result.RequestMessage.Method.Method)
                  //                     .InserirUrlDaRequisicao(message.Result.RequestMessage.RequestUri.AbsoluteUri)
                  //                     .InserirCriacaoDeLog(true);
                  }
              });
        }
    }
}
