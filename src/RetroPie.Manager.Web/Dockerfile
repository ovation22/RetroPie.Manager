#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage will be used for debugging
FROM mcr.microsoft.com/dotnet/aspnet:9.0-alpine AS debug
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0-alpine AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/RetroPie.Manager.Web/RetroPie.Manager.Web.csproj", "RetroPie.Manager.Web/"]
COPY ["src/RetroPie.Manager.Web.Client/RetroPie.Manager.Web.Client.csproj", "RetroPie.Manager.Web.Client/"]
RUN dotnet restore "./RetroPie.Manager.Web/./RetroPie.Manager.Web.csproj"
COPY . .
WORKDIR "/src/RetroPie.Manager.Web"
RUN dotnet build "./RetroPie.Manager.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RetroPie.Manager.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage will be used for production and as such is based on the chiseled Ubuntu image
FROM mcr.microsoft.com/dotnet/runtime:9.0-jammy-chiseled AS final
USER app
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RetroPie.Manager.Web.dll"]