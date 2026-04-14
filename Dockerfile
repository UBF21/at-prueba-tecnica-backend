# Multi-stage build para .NET 9
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos de solución y proyecto
COPY ["at-prueba-tecnica-backend.sln", "."]
COPY ["at-prueba-tecnica-backend.Domain/at-prueba-tecnica-backend.Domain.csproj", "at-prueba-tecnica-backend.Domain/"]
COPY ["at-prueba-tecnica-backend.Application/at-prueba-tecnica-backend.Application.csproj", "at-prueba-tecnica-backend.Application/"]
COPY ["at-prueba-tecnica-backend.Infrastructure/at-prueba-tecnica-backend.Infrastructure.csproj", "at-prueba-tecnica-backend.Infrastructure/"]
COPY ["at-prueba-tecnica-backend.Api/at-prueba-tecnica-backend.Api.csproj", "at-prueba-tecnica-backend.Api/"]

# Restaurar dependencias
RUN dotnet restore

# Copiar el resto del código
COPY . .

# Publicar en Release
RUN dotnet publish "at-prueba-tecnica-backend.Api/at-prueba-tecnica-backend.Api.csproj" \
    -c Release \
    -o /app/publish

# Stage de runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copiar archivos publicados del stage anterior
COPY --from=build /app/publish .

# Instalar curl y herramientas para migraciones
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Copiar SDK de .NET para ejecutar migraciones (opcional, ya está incluido en build stage)
# COPY --from=build /usr/local/bin/dotnet /usr/local/bin/dotnet

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "at-prueba-tecnica-backend.Api.dll"]
