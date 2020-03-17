#!/bin/bash

set -x

dotnet test Currency.Service.Tests/Currency.Service.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../coverage/lcov $*
