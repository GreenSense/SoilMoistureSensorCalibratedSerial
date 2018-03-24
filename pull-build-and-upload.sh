#!/bin/bash

echo ""
echo "Pulling git changes, building, and uploading..."
echo ""

PORT_NAME=$1

if [ ! $PORT_NAME ]; then
  PORT_NAME="ttyUSB0"
fi

echo "Port: $PORT_NAME"

git checkout master && \
git pull origin master && \
sh inject-version.sh && \
sh build-and-upload.sh $PORT_NAME
