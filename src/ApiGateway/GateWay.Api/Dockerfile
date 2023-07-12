FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["ApiGateway/GateWay.Api/GateWay.Api.csproj", "ApiGateway/GateWay.Api/GateWay.Api.csproj"]
COPY ["ApiGateway/GateWay.Api/GateWay.Api.csproj", "ApiGateway/GateWay.Api/GateWay.Api.csproj"]
RUN dotnet restore "ApiGateway/GateWay.Api/GateWay.Api.csproj"
COPY . .
WORKDIR "/src/GateWay.Api"
RUN dotnet build "GateWay.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GateWay.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GateWay.Api.dll"]
