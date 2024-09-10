FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER www-data
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 9080
EXPOSE 9443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["IdentityProvider-API/IdentityProvider-API.csproj", "IdentityProvider-API/"]
RUN dotnet restore "IdentityProvider-API/IdentityProvider-API.csproj"
COPY . .
WORKDIR "/src/IdentityProvider-API"
RUN dotnet build "IdentityProvider-API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "IdentityProvider-API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
WORKDIR /app
COPY ./IdentityProvider-API/Certificate/localhost.pfx /app/publish/Certificate/localhost.pfx

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN chown -R www-data:www-data /app/keys
RUN chmod -R 755 /app/keys

USER www-data
ENTRYPOINT ["dotnet", "IdentityProvider-API.dll"]