FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./WebVeiculos.Models/WebVeiculos.Models.csproj WebVeiculos.Models/
COPY ./WebVeiculos.Core/WebVeiculos.Core.csproj WebVeiculos.Core/
COPY ./WebVeiculos.WebApi/WebVeiculos.WebApi.csproj WebVeiculos.WebApi/

RUN dotnet restore ./WebVeiculos.WebApi/WebVeiculos.WebApi.csproj

# Copy everything else and build
COPY . ./
RUN dotnet publish ./WebVeiculos.WebApi -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:5.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WebVeiculos.WebApi.dll"]