FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GcarRateLimit.Web/GcarRateLimit.Web.csproj", "src/GcarRateLimit.Web/"]
COPY ["src/GcarRateLimit.RateLimiter/GcarRateLimit.RateLimiter.csproj", "src/GcarRateLimit.RateLimiter/"]
RUN dotnet restore "src/GcarRateLimit.Web/GcarRateLimit.Web.csproj"
COPY . .
WORKDIR "/src/src/GcarRateLimit.Web"
RUN dotnet build "GcarRateLimit.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GcarRateLimit.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GcarRateLimit.Web.dll"]
