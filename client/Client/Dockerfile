FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["client/Client/Client.csproj", "client/Client/"]
COPY ["client/modules/Authentication/Authentication.csproj", "client/modules/Authentication/"]
COPY ["client/ClientShared/ClientShared.csproj", "client/ClientShared/"]
COPY ["shared/Shared/Shared.csproj", "shared/Shared/"]
COPY ["client/modules/Learning/Learning.csproj", "client/modules/Learning/"]
RUN dotnet restore "client/Client/Client.csproj"
COPY . .
WORKDIR "/src/client/Client"
RUN dotnet build "./Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Client.dll"]
