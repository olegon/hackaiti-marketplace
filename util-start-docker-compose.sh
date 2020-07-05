#!/bin/bash

if [ -z "$AWS_ACCESS_KEY_ID" ]
then
    echo "You must specify AWS_ACCESS_KEY_ID environment variable"
    exit 1
fi

if [ -z "$AWS_SECRET_ACCESS_KEY" ]
then
    echo "You must specify AWS_SECRET_ACCESS_KEY environment variable"
    exit 1
fi

if [ -z "$AWS_REGION" ]
then
    echo "You must specify AWS_REGION environment variable"
    exit 1
fi

export USER_ID=$(id -u)

set -x

docker-compose up --build --force-recreate $*
