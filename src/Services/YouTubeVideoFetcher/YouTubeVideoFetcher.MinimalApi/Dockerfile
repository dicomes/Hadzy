FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Hadzy-Services.sln", "Hadzy-Services.sln"]
COPY ["Services/YouTubeVideoFetcher/YouTubeVideoFetcher.MinimalApi/YouTubeVideoFetcher.MinimalApi.csproj", "Services/YouTubeVideoFetcher/YouTubeVideoFetcher.MinimalApi/YouTubeVideoFetcher.MinimalApi.csproj"]
RUN dotnet restore "Hadzy-Services.sln"

COPY . .
WORKDIR "src/Services/YouTubeVideoFetcher/YouTubeVideoFetcher.MinimalApi"
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "YouTubeVideoFetcher.MinimalApi.dll"]
