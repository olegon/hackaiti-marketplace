#!/bin/bash

set -x

docker build -t local/Currency.Service . $*
