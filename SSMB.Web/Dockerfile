FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.1-stretch AS build
WORKDIR /src
COPY ["StarSonataMarketBoard/StarSonataMarketBoard.csproj", "StarSonataMarketBoard/"]
RUN dotnet restore "StarSonataMarketBoard/StarSonataMarketBoard.csproj"
COPY . .
WORKDIR "/src/StarSonataMarketBoard"
RUN dotnet build "StarSonataMarketBoard.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "StarSonataMarketBoard.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "StarSonataMarketBoard.dll"]