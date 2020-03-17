FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Product.Service.sln ./
COPY Product.Service.API/Product.Service.API.csproj ./Product.Service.API/Product.Service.API.csproj
COPY Product.Service.Tests/Product.Service.Tests.csproj ./Product.Service.Tests/Product.Service.Tests.csproj
RUN dotnet restore

# Copy everything else
COPY . ./

# Run unit tests
RUN dotnet test Product.Service.Tests/Product.Service.Tests.csproj /p:CollectCoverage=true

# Build aplication release
RUN dotnet publish ./Product.Service.API/Product.Service.API.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Product.Service.API.dll"]
