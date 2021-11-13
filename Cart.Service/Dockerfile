FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Cart.Service.sln ./
COPY Cart.Service.API/Cart.Service.API.csproj ./Cart.Service.API/Cart.Service.API.csproj
COPY Cart.Service.Tests/Cart.Service.Tests.csproj ./Cart.Service.Tests/Cart.Service.Tests.csproj
RUN dotnet restore

# Copy everything else
COPY . ./

# Run unit tests
# (disabled while theres no test) RUN dotnet test Cart.Service.Tests/Cart.Service.Tests.csproj /p:CollectCoverage=true

# Build aplication release
RUN dotnet publish ./Cart.Service.API/Cart.Service.API.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Cart.Service.API.dll"]
