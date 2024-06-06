#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Menza.Server/Menza.Server.csproj", "Menza.Server/"]
COPY ["Menza.Shared/Menza.Shared.csproj", "Menza.Shared/"]
COPY ["Menza.Client/Menza.Client.csproj", "Menza.Client/"]
RUN dotnet restore "Menza.Server/Menza.Server.csproj"
COPY . .
WORKDIR "/src/Menza.Server"
RUN dotnet build -c Release -o /app/build

FROM build AS publish
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Menza.Server.dll"]
