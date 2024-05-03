#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["VolleyLeague.API/VolleyLeague.API.csproj", "VolleyLeague.API/"]
COPY ["VolleyLeague.Client.Blazor/VolleyLeague.Client.Blazor.csproj", "VolleyLeague.Client.Blazor/"]
COPY ["VolleyLeague.Entities/VolleyLeague.Entities.csproj", "VolleyLeague.Entities/"]
COPY ["VolleyLeague.Repositories/VolleyLeague.Repositories.csproj", "VolleyLeague.Repositories/"]
COPY ["VolleyLeague.Services/VolleyLeague.Services.csproj", "VolleyLeague.Services/"]
RUN dotnet restore "./VolleyLeague.API/./VolleyLeague.API.csproj"
COPY . .
WORKDIR "/src/VolleyLeague.API"
RUN dotnet build "./VolleyLeague.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./VolleyLeague.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VolleyLeague.API.dll"]