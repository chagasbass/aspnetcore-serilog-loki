using Microsoft.AspNetCore.Http;
using RestoqueMinimal.Extensions.Entities;

namespace RestoqueMinimal.Extensions.CustomResults
{
    public interface IApiCustomResults
    {
        void GenerateLogResponse(CommandResult commandResult, int statusCode);
        IResult FormatApiResponse(CommandResult commandResult, string? defaultEndpoint = null);
    }
}