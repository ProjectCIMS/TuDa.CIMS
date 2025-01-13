FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
ARG TARGETARCH
COPY ["infrastructure/TuDa.CIMS.ExcelImporter/TuDa.CIMS.ExcelImporter.csproj", "infrastructure/TuDa.CIMS.ExcelImporter/"]
COPY ["src/TuDa.CIMS.Shared/TuDa.CIMS.Shared.csproj", "src/TuDa.CIMS.Shared/"]
COPY ["test/TuDa.CIMS.Shared.Test/TuDa.CIMS.Shared.Test.csproj", "test/TuDa.CIMS.Shared.Test/"]
COPY ["infrastructure/TuDa.CIMS.ServiceDefaults/TuDa.CIMS.ServiceDefaults.csproj", "infrastructure/TuDa.CIMS.ServiceDefaults/"]
COPY ["src/TuDa.CIMS.Api/TuDa.CIMS.Api.csproj", "src/TuDa.CIMS.Api/"]
COPY ["infrastructure/TuDa.CIMS.MigrationService/TuDa.CIMS.MigrationService.csproj", "infrastructure/TuDa.CIMS.MigrationService/"]
RUN dotnet restore "infrastructure/TuDa.CIMS.MigrationService/TuDa.CIMS.MigrationService.csproj" -a $TARGETARCH

COPY . .
WORKDIR "/src/infrastructure/TuDa.CIMS.MigrationService"
RUN dotnet build "TuDa.CIMS.MigrationService.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
RUN dotnet publish "TuDa.CIMS.MigrationService.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -a $TARGETARCH

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TuDa.CIMS.MigrationService.dll"]
