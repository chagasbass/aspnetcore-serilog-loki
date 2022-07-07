using Microsoft.AspNetCore.Mvc;
using RestoqueMinimal.Extensions.CustomResults;
using RestoqueMinimal.Extensions.Documentations;
using RestoqueMinimal.Extensions.Entities;
using RestoqueMinimal.Extensions.Logs;
using RestoqueMinimal.Extensions.Middlewares;
using RestoqueMinimal.Extensions.Observability.Healthchecks;
using RestoqueMinimal.Extensions.Options;
using RestoqueMinimal.Extensions.Performances;
using RestoqueMinimal.Extensions.Shared.Enums;
using RestoqueMinimal.Extensions.Shared.Logs.Services;
using RestoqueMinimal.Extensions.Shared.Notifications;
using Serilog;
using SerilogLoki.Api.API.Extensions;


var builder = WebApplication.CreateBuilder(args);


var configuration = builder.Configuration;

Log.Logger = SerilogLokiIntegrationExtensions.ConfigureStructuralLogWithSerilogAndLoki(configuration,builder);
builder.Logging.AddSerilog(Log.Logger);

try
{
    #region configuracoes das extensoes

    /*A extension do application Insights s� deve ser inserida quando a chave estiver no appSettings
    *.AddApplicationInsightsApiTelemetry(configuration)
    * 
    *Caso seja necess�rio usar a extension para resili�ncia em chamadas htttp ativar a extension
    *AddWorkerResiliencesPatterns
    * 
    * Existe uma implementa��o de log mais simples usando o padr�o
    * do aspnetCore. Caso queira usa-lo 
    * as configura��es do serilog devem ser retiradas e o servi�o de log deve ser reimplementado
    * o try catch n�o � mais necess�rio
    * retirar a execu��o da extension :
    * adicionar na pilha a extension AddMinimalApiAspNetCoreHttpLogging
    */

    builder.Services.AddEndpointsApiExplorer()
                .AddBaseConfigurationOptionsPattern(configuration)
                .AddSwaggerDocumentation(configuration)
                .AddLogServiceDependencies()
                .AddNotificationControl()
                .AddAppHealthChecks()
                .AddMinimalApiPerformanceBoost()
                .AddDependencyInjection()
                .AddApiCustomResults()
                .AddGlobalExceptionHandlerMiddleware()
                .AddApiVersioning(x => x.DefaultApiVersion = ApiVersion.Default);

    #endregion

    var app = builder.Build();

    #region configuracoes dos middlewares

    app.UseResponseCompression()
       .UseMiddleware<GlobalExceptionHandlerMiddleware>()
       .UseMiddleware<SerilogRequestLoggerMiddleware>()
       .UseSwagger()
       .UseSwaggerUI()
       .UseHealthChecks(configuration)
       .UseHttpsRedirection();


    #endregion

    MapApiActions(app);

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminado inexperadamente.");
}
finally
{
    Log.CloseAndFlush();
}

//os endpoints devem ser colocados dentro desse m�todo.
void MapApiActions(WebApplication app)
{
    //endpoint get de exemplo retornando status BadRequest no problemDetail dentro do CommandResult
    app.MapGet("/log-information", (IApiCustomResults customResults, INotificationServices notificationServices) =>
    {
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool" };

        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateTime.Now.AddDays(index),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();

        notificationServices.AddNotification(new Flunt.Notifications.Notification("Propriedade", "Erro encontrado"),
                                             StatusCodeOperation.BadRequest);

        var commandResult = new CommandResult(forecast, false, "erro no processo de busca do weatherForecast");

        return customResults.FormatApiResponse(commandResult);
    })
      .Produces(StatusCodes.Status404NotFound)
      .Produces<CommandResult>(StatusCodes.Status200OK)
      .ProducesProblem(StatusCodes.Status500InternalServerError)
      .WithName("GetLogInformation")
      .WithTags("log-information")
      .WithMetadata("teste metadata");

    //endpoint get de exemplo retornando status BadRequest no problemDetail dentro do CommandResult
    app.MapGet("/log-error", (IApiCustomResults customResults, INotificationServices notificationServices) =>
    {
        var summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool" };

        var forecast = Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateTime.Now.AddDays(index),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            ))
            .ToArray();

        notificationServices.AddNotification(new Flunt.Notifications.Notification("Propriedade", "Erro encontrado"),
                                             StatusCodeOperation.BadRequest);

        throw new Exception("Exemplo de erro no fluxo");

        var commandResult = new CommandResult(forecast, false, "erro no processo de busca do weatherForecast");

        return customResults.FormatApiResponse(commandResult);
    })
      .Produces(StatusCodes.Status404NotFound)
      .Produces<CommandResult>(StatusCodes.Status200OK)
      .ProducesProblem(StatusCodes.Status500InternalServerError)
      .WithName("GetLogIErrors")
      .WithTags("log-error")
      .WithMetadata("teste metadata");
}

internal record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}