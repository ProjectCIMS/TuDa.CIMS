FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
ARG TARGETARCH
COPY ["src/TuDa.CIMS.Shared/TuDa.CIMS.Shared.csproj", "src/TuDa.CIMS.Shared/"]
RUN dotnet restore "src/TuDa.CIMS.Shared/TuDa.CIMS.Shared.csproj" -a $TARGETARCH
COPY ["src/TuDa.CIMS.Web/TuDa.CIMS.Web.csproj", "src/TuDa.CIMS.Web/"]
RUN dotnet restore "src/TuDa.CIMS.Web/TuDa.CIMS.Web.csproj" -a $TARGETARCH
COPY . .
WORKDIR "/src/src/TuDa.CIMS.Web"
RUN dotnet build "TuDa.CIMS.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build --no-restore

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
RUN dotnet publish "TuDa.CIMS.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -a $TARGETARCH --no-restore

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TuDa.CIMS.Web.dll"]
