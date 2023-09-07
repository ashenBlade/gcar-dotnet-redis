FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/GcraRateLimit.Producer/GcraRateLimit.Producer.csproj", "src/GcraRateLimit.Producer/"]
COPY ["src/GcraRateLimit.RateLimiter/GcraRateLimit.RateLimiter.csproj", "src/GcraRateLimit.RateLimiter/"]
RUN dotnet restore "src/GcraRateLimit.Producer/GcraRateLimit.Producer.csproj"
COPY . .
WORKDIR "/src/src/GcraRateLimit.Producer"
RUN dotnet build "GcraRateLimit.Producer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GcraRateLimit.Producer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GcraRateLimit.Producer.dll"]
