FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY Checkout.Service.sln ./
COPY Checkout.Service.Worker/Checkout.Service.Worker.csproj ./Checkout.Service.Worker/Checkout.Service.Worker.csproj
COPY Checkout.Service.UnitTests/Checkout.Service.UnitTests.csproj ./Checkout.Service.UnitTests/Checkout.Service.UnitTests.csproj
RUN dotnet restore

# Copy everything else
COPY . ./

# Run unit tests
# (disabled while theres no test) RUN dotnet test Checkout.Service.UnitTests/Checkout.Service.UnitTests.csproj /p:CollectCoverage=true

# Build aplication release
RUN dotnet publish ./Checkout.Service.Worker/Checkout.Service.Worker.csproj -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/runtime:6.0
EXPOSE 8080
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "Checkout.Service.Worker.dll"]
