#!/bin/bash

set -x

docker build -t local/hackaiti-webapi-template . $*
