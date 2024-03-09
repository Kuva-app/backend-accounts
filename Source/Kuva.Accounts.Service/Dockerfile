#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Kuva.Accounts.Service/Kuva.Accounts.Service.csproj", "Kuva.Accounts.Service/"]
COPY ["Kuva.Accounts.Business/Kuva.Accounts.Business.csproj", "Kuva.Accounts.Business/"]
COPY ["Kuva.Accounts.Entities/Kuva.Accounts.Entities.csproj", "Kuva.Accounts.Entities/"]
COPY ["Kuva.Accounts.Repository/Kuva.Accounts.Repository.csproj", "Kuva.Accounts.Repository/"]
RUN dotnet restore "./Kuva.Accounts.Service/Kuva.Accounts.Service.csproj"
COPY . .
WORKDIR "/src/Kuva.Accounts.Service"
RUN dotnet build "./Kuva.Accounts.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Kuva.Accounts.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kuva.Accounts.Service.dll"]