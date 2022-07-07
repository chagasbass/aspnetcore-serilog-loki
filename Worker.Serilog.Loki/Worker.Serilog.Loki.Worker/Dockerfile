#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Worker.Serilog.Loki.Worker/Worker.Serilog.Loki.Worker.csproj", "Worker.Serilog.Loki.Worker/"]
COPY ["Worker.Serilog.Loki.Extensions/Worker.Serilog.Loki.Extensions.csproj", "Worker.Serilog.Loki.Extensions/"]
COPY ["Worker.Serilog.Loki.Shared/Worker.Serilog.Loki.Shared.csproj", "Worker.Serilog.Loki.Shared/"]
COPY ["Worker.Serilog.Loki.Tests/Worker.Serilog.Loki.Tests.csproj", "Worker.Serilog.Loki.Tests/"]
COPY ["Worker.Serilog.Loki.Domain/Worker.Serilog.Loki.Domain.csproj", "Worker.Serilog.Loki.Domain/"]
COPY ["Worker.Serilog.Loki.Infra.Data/Worker.Serilog.Loki.Infra.Data.csproj", "Worker.Serilog.Loki.Infra.Data/"]
RUN dotnet restore "Worker.Serilog.Loki.Worker/Worker.Serilog.Loki.Worker.csproj"
COPY . .
WORKDIR "/src/Worker.Serilog.Loki.Worker"
RUN dotnet build "Worker.Serilog.Loki.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Worker.Serilog.Loki.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Worker.Serilog.Loki.Worker.dll"]

