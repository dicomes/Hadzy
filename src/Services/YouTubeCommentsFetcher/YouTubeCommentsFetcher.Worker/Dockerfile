FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["Hadzy-Services.sln", "Hadzy-Services.sln"]
COPY ["Services/YouTubeCommentsFetcher/YouTubeCommentsFetcher.Worker/YouTubeCommentsFetcher.Worker.csproj", "Services/YouTubeCommentsFetcher/YouTubeCommentsFetcher.Worker/YouTubeCommentsFetcher.Worker.csproj"]
RUN dotnet restore "Hadzy-Services.sln"

COPY . .
WORKDIR "src/Services/YouTubeCommentsFetcher/YouTubeCommentsFetcher.Worker"
RUN dotnet publish --no-restore -c Release -o /app

FROM build AS publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "YouTubeCommentsFetcher.Worker.dll"]
