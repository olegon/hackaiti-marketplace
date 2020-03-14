#!/bin/bash

set -x

dotnet test Cart.Service.Tests/Cart.Service.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../coverage/lcov $*
