#!/bin/bash

set -x

dotnet test Product.Service.Tests/Product.Service.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../coverage/lcov $*
