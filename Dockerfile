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

# Binding is configured at runtime in Program.cs from Render's $PORT
# (default 10000). Clear the image default to avoid double-binding.
ENV ASPNETCORE_HTTP_PORTS=
EXPOSE 10000
ENTRYPOINT ["dotnet", "evaluacion20262.dll"]