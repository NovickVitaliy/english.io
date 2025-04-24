FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["english-io-bot/English.IO.Bot/English.IO.Bot.csproj", "english-io-bot/English.IO.Bot/"]
COPY ["shared/Shared.MessageBus/Shared.MessageBus.csproj", "shared/Shared.MessageBus/"]
COPY ["/Directory.Packages.props","/"]
RUN dotnet restore "english-io-bot/English.IO.Bot/English.IO.Bot.csproj"
COPY . .
WORKDIR "/src/english-io-bot/English.IO.Bot"
RUN dotnet build "English.IO.Bot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "English.IO.Bot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "English.IO.Bot.dll"]
