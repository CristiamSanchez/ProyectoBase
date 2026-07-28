# =========================
# Etapa 1: Build
# =========================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY ["src/Proyecto.Api/Proyecto.Api.csproj", "src/Proyecto.Api/"]
COPY ["src/Proyecto.Application/Proyecto.Application.csproj", "src/Proyecto.Application/"]
COPY ["src/Proyecto.Domain/Proyecto.Domain.csproj", "src/Proyecto.Domain/"]
COPY ["src/Proyecto.Infrastructure/Proyecto.Infrastructure.csproj", "src/Proyecto.Infrastructure/"]

RUN dotnet restore "src/Proyecto.Api/Proyecto.Api.csproj"

COPY . .

WORKDIR "/src/src/Proyecto.Api"

RUN dotnet build "Proyecto.Api.csproj" \
    -c Release \
    -o /app/build

# =========================
# Etapa 2: Publish
# =========================
FROM build AS publish

RUN dotnet publish "Proyecto.Api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false

# =========================
# Etapa 3: Runtime
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 8080

ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Proyecto.Api.dll"]