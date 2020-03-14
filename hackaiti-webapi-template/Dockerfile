FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY hackaiti-webapi-template.sln ./
COPY hackaiti-webapi-template.API/hackaiti-webapi-template.API.csproj ./hackaiti-webapi-template.API/hackaiti-webapi-template.API.csproj
COPY hackaiti-webapi-template.Tests/hackaiti-webapi-template.Tests.csproj ./hackaiti-webapi-template.Tests/hackaiti-webapi-template.Tests.csproj
RUN dotnet restore

# Copy everything else
COPY . ./

# Run unit tests
RUN dotnet test hackaiti-webapi-template.Tests/hackaiti-webapi-template.Tests.csproj /p:CollectCoverage=true

# Build aplication release
RUN dotnet publish ./hackaiti-webapi-template.API/hackaiti-webapi-template.API.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "hackaiti-webapi-template.API.dll"]
