# cd src/Menza && docker build -t menza .
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR "/src"
COPY ["Menza.Server/Menza.Server.csproj", "Menza.Server/"]
COPY ["Menza.Shared/Menza.Shared.csproj", "Menza.Shared/"]
COPY ["Menza.Client/Menza.Client.csproj", "Menza.Client/"]
RUN dotnet restore "Menza.Server/Menza.Server.csproj"
COPY . .
WORKDIR "/src/Menza.Server"
RUN dotnet build "Menza.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Menza.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Menza.Server.dll"]
