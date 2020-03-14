#!/bin/bash

set -x

docker build -t local/product-service . $*
