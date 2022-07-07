#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SerilogLoki.Api.csproj", "."]
COPY ["SerilogLoki.Api.Api/SerilogLoki.Api.Api", "SerilogLoki.Api.Api/"]
COPY ["SerilogLoki.Api.Tests/SerilogLoki.Api.Tests.csproj", "SerilogLoki.Api.Tests/"]
COPY ["RestoqueMinimal.Extensions/RestoqueMinimal.Extensions.csproj", "RestoqueMinimal.Extensions/"]
RUN dotnet restore "./SerilogLoki.Api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "SerilogLoki.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SerilogLoki.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SerilogLoki.Api.dll"]