FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["src/Backend/EsperancaSolidaria.Identity.API/EsperancaSolidaria.Identity.API.csproj", "Backend/EsperancaSolidaria.Identity.API/"]
COPY ["src/Backend/EsperancaSolidaria.Identity.Application/EsperancaSolidaria.Identity.Application.csproj", "Backend/EsperancaSolidaria.Identity.Application/"]
COPY ["src/Backend/EsperancaSolidaria.Identity.Domain/EsperancaSolidaria.Identity.Domain.csproj", "Backend/EsperancaSolidaria.Identity.Domain/"]
COPY ["src/Backend/EsperancaSolidaria.Identity.Infrastructure/EsperancaSolidaria.Identity.Infrastructure.csproj", "Backend/EsperancaSolidaria.Identity.Infrastructure/"]
COPY ["src/Shared/EsperancaSolidaria.Identity.Communication/EsperancaSolidaria.Identity.Communication.csproj", "Shared/EsperancaSolidaria.Identity.Communication/"]
COPY ["src/Shared/EsperancaSolidaria.Identity.Exceptions/EsperancaSolidaria.Identity.Exceptions.csproj", "Shared/EsperancaSolidaria.Identity.Exceptions/"]

RUN dotnet restore "Backend/EsperancaSolidaria.Identity.API/EsperancaSolidaria.Identity.API.csproj"

COPY src/ .

RUN dotnet publish "Backend/EsperancaSolidaria.Identity.API/EsperancaSolidaria.Identity.API.csproj" \
    -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EsperancaSolidaria.Identity.API.dll"]