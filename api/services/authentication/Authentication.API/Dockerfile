FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["api/services/authentication/Authentication.API/Authentication.API.csproj", "api/services/authentication/Authentication.API/"]
COPY ["shared/Shared/Shared.csproj", "shared/Shared/"]
RUN dotnet restore "api/services/authentication/Authentication.API/Authentication.API.csproj"
COPY . .
WORKDIR "/src/api/services/authentication/Authentication.API"
RUN dotnet build "Authentication.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Authentication.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.API.dll"]
