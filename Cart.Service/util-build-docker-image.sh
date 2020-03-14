#!/bin/bash

set -x

docker build -t local/cart-service . $*
