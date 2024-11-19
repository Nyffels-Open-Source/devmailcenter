FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-dotnet
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["DevMailCenter.Api/DevMailCenter.Api.csproj", "DevMailCenter.Api/"]
RUN dotnet restore "./DevMailCenter.Api/DevMailCenter.Api.csproj"
COPY . .
WORKDIR "/src/DevMailCenter.Api"
RUN dotnet build "./DevMailCenter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM node:lts-alpine AS build-angular
WORKDIR /src
COPY ./DevMailCenter.Client .
RUN npm install -g @angular/cli
RUN npm ci 
RUN npm run build

FROM build-dotnet AS publish-dotnet
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./DevMailCenter.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM build-angular AS publish-angular

FROM base AS final
WORKDIR /app
COPY --from=publish-dotnet /app/publish .
ENTRYPOINT ["dotnet", "DevMailCenter.Api.dll"]