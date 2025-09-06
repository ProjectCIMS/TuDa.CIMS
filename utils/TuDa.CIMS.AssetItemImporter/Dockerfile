FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
ARG TARGETARCH

# Restore dependencies with layer caching
COPY ["src/TuDa.CIMS.Shared/TuDa.CIMS.Shared.csproj", "src/TuDa.CIMS.Shared/"]
RUN dotnet restore "src/TuDa.CIMS.Shared/TuDa.CIMS.Shared.csproj" -a $TARGETARCH

COPY ["src/TuDa.CIMS.Web/TuDa.CIMS.Web.csproj", "src/TuDa.CIMS.Web/"]
RUN dotnet restore "src/TuDa.CIMS.Web/TuDa.CIMS.Web.csproj" -a $TARGETARCH

COPY ["utils/TuDa.CIMS.AssetItemImporter/TuDa.CIMS.AssetItemImporter.csproj", "utils/TuDa.CIMS.AssetItemImporter/"]
RUN dotnet restore "utils/TuDa.CIMS.AssetItemImporter/TuDa.CIMS.AssetItemImporter.csproj" -a $TARGETARCH

# Copy the entire source and build
COPY . .
WORKDIR "/src/utils/TuDa.CIMS.AssetItemImporter"
RUN dotnet build "TuDa.CIMS.AssetItemImporter.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
ARG TARGETARCH
RUN dotnet publish "TuDa.CIMS.AssetItemImporter.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -a $TARGETARCH

FROM base AS final

# == Set the locale to German ==
USER root
RUN apt-get update && apt-get install -y locales \
    && sed -i '/de_DE.UTF-8/s/^# //g' /etc/locale.gen \
    && locale-gen de_DE.UTF-8 \
    && rm -rf /var/lib/apt/lists/*

ENV LANG=de_DE.UTF-8
ENV LANGUAGE=de_DE:de
ENV LC_ALL=de_DE.UTF-8
# == End of setting the locale ==

USER $APP_UID
WORKDIR /app
COPY --from=publish /app/publish .

# Convenience CLI wrapper inside the image
COPY utils/TuDa.CIMS.AssetItemImporter/container/asset-importer.sh /usr/local/bin/asset-importer
RUN chmod +x /usr/local/bin/asset-importer

# Run the CLI by default; users can pass args. If no args are provided,
# show help to make the container self-explanatory.
ENTRYPOINT ["dotnet", "TuDa.CIMS.AssetItemImporter.dll"]
CMD ["--help"]
