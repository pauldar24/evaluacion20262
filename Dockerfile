# Multi-stage build pipeline for Render (Linux container)
# Base images: Ubuntu 24.04 (Noble). Alpine/musl must NOT be used here:
# SQLitePCLRaw's bundled native e_sqlite3 is built against glibc and
# segmentation-faults on Alpine (exit 139).
FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build
WORKDIR /src

# Restore first for better layer caching
COPY ["evaluacion20262.csproj", "."]
RUN dotnet restore "evaluacion20262.csproj"

COPY . .
RUN dotnet publish "evaluacion20262.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:10.0-noble AS final

# Install SQLite system libraries (Debian/Ubuntu equivalent of
# 'apt-get install -y sqlite3 libsqlite3-dev', without dev headers)
RUN apt-get update \
    && apt-get install -y --no-install-recommends sqlite3 libsqlite3-0 \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=build /app/publish .

# Binding is configured at runtime in Program.cs from Render's $PORT
# (default 10000). Clear the image default to avoid double-binding.
ENV ASPNETCORE_HTTP_PORTS=
EXPOSE 10000
ENTRYPOINT ["dotnet", "evaluacion20262.dll"]