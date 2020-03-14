#!/bin/bash

set -x

dotnet test hackaiti-webapi-template.Tests/hackaiti-webapi-template.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../coverage/lcov $*
