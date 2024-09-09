FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 80
EXPOSE 443
EXPOSE 6080
EXPOSE 6443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["API/API.csproj", "API/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
RUN dotnet restore "API/API.csproj"
COPY . .
WORKDIR "/src/API"
RUN dotnet build "API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
COPY ./API/Certificate/localhost.pfx /app/publish/Certificate/localhost.pfx
COPY ./Infrastructure/SeedData/users.json /app/SeedData/users.json
COPY ./Infrastructure/SeedData/vehicleBrands.json /app/publish/SeedData/vehicleBrands.json
COPY ./Infrastructure/SeedData/vehicleOwners.json /app/publish/SeedData/vehicleOwners.json
COPY ./Infrastructure/SeedData/vehicles.json /app/publish/SeedData/vehicles.json

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
