FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TuDa.CIMS.Api/TuDa.CIMS.Api.csproj", "TuDa.CIMS.Api/"]
COPY ["TuDa.CIMS.Shared/TuDa.CIMS.Shared.csproj", "TuDa.CIMS.Shared/"]
RUN dotnet restore "TuDa.CIMS.Api/TuDa.CIMS.Api.csproj"
COPY . .
WORKDIR "/src/TuDa.CIMS.Api"
RUN dotnet build "TuDa.CIMS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TuDa.CIMS.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TuDa.CIMS.Api.dll"]
