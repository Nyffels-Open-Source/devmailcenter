FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY ./build/ .
ENTRYPOINT ["dotnet", "DevMailCenter.Api.dll"]