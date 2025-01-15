FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["api/services/learning/Learning.API/Learning.API.csproj", "api/services/learning/Learning.API/"]
COPY ["api/services/learning/Learning.Application/Learning.Application.csproj", "api/services/learning/Learning.Application/"]
COPY ["api/services/learning/Learning.Domain/Learning.Domain.csproj", "api/services/learning/Learning.Domain/"]
COPY ["shared/Shared/Shared.csproj", "shared/Shared/"]
COPY ["api/services/learning/Learning.Infrastructure/Learning.Infrastructure.csproj", "api/services/learning/Learning.Infrastructure/"]
COPY ["shared/Shared.MessageBus/Shared.MessageBus.csproj", "shared/Shared.MessageBus/"]
COPY ["shared/Shared.Authentication/Shared.Authentication.csproj", "shared/Shared.Authentication/"]
RUN dotnet restore "api/services/learning/Learning.API/Learning.API.csproj"
COPY . .
WORKDIR "/src/api/services/learning/Learning.API"
RUN dotnet build "Learning.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Learning.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Learning.API.dll"]
