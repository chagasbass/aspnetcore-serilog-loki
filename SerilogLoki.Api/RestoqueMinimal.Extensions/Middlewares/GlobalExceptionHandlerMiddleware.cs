using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestoqueMinimal.Extensions.Entities;
using RestoqueMinimal.Extensions.Factories;
using RestoqueMinimal.Extensions.Shared.Configurations;
using RestoqueMinimal.Extensions.Shared.Enums;
using RestoqueMinimal.Extensions.Shared.Logs.Services;
using System.Text.Json;

namespace RestoqueMinimal.Extensions.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ProblemDetailConfigurationOptions _problemOptions;
        private readonly ILogServices _logServices;

        public GlobalExceptionHandlerMiddleware(IOptions<ProblemDetailConfigurationOptions> problemOptions,
                                                ILogServices logServices)
        {
            _problemOptions = problemOptions.Value;
            _logServices = logServices;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        /// <summary>
        /// Responsavel por tratar as exceções geradas na aplicação
        /// </summary>
        /// <param name="context"></param>
        /// <param name="exception"></param>
        /// <returns></returns>
        public async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            const int statusCode = StatusCodes.Status500InternalServerError;
            const string dataType = @"application/problem+json";

            _logServices.LogData.AddException(exception);
            _logServices.LogData.AddResponseStatusCode(statusCode);
            _logServices.WriteLog();
            _logServices.WriteLogWhenRaiseExceptions();

            var problemDetails = ConfigurarDetalhesDoProblema(statusCode, exception, context);

            var commandResult = new CommandResult(problemDetails);

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = dataType;

            await context.Response.WriteAsync(JsonSerializer.Serialize(commandResult, JsonOptionsFactory.GetSerializerOptions()));
        }

        private ProblemDetails ConfigurarDetalhesDoProblema(int statusCode, Exception exception, HttpContext context)
        {
            var defaultTitle = "Um erro ocorreu ao processar o request.";
            var defaultDetail = $"Erro fatal na aplicação,entre em contato com um Desenvolvedor responsável. Causa: {exception.Message}";

            var title = _problemOptions.Title;
            var detail = _problemOptions.Detail;
            var instance = context.Request.HttpContext.Request.Path.ToString();

            var type = StatusCodeOperation.RetrieveStatusCode(statusCode);

            if (string.IsNullOrEmpty(title))
                title = defaultTitle;

            if (string.IsNullOrEmpty(detail))
                detail = defaultDetail;

            return new ProblemDetails()
            {
                Detail = detail,
                Instance = instance,
                Status = statusCode,
                Title = title,
                Type = type.Text
            };
        }
    }
}
