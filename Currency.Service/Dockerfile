FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Currency.Service.sln ./
COPY Currency.Service.API/Currency.Service.API.csproj ./Currency.Service.API/Currency.Service.API.csproj
COPY Currency.Service.Tests/Currency.Service.Tests.csproj ./Currency.Service.Tests/Currency.Service.Tests.csproj
RUN dotnet restore

# Copy everything else
COPY . ./

# Run unit tests
# (disabled while theres no test) RUN dotnet test Currency.Service.Tests/Currency.Service.Tests.csproj /p:CollectCoverage=true

# Build aplication release
RUN dotnet publish ./Currency.Service.API/Currency.Service.API.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Currency.Service.API.dll"]
