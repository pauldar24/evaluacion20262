# Multi-stage build pipeline for Render (Linux container)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Restore first for better layer caching
COPY ["evaluacion20262.csproj", "."]
RUN dotnet restore "evaluacion20262.csproj"

COPY . .
RUN dotnet publish "evaluacion20262.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# Render web services must bind to $PORT at runtime (default 10000).
# Cache-busting note: the shell form evaluates ${PORT:-8080} at container start.
EXPOSE 8080
ENTRYPOINT ["/bin/sh", "-c", "ASPNETCORE_URLS=\"http://+:${PORT:-8080}\" dotnet evaluacion20262.dll"]