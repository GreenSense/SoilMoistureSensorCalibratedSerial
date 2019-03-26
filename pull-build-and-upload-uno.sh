#!/bin/bash

echo ""
echo "Pulling git changes, building, and uploading..."
echo ""

PORT_NAME="/dev/$1"

if [ ! $PORT_NAME ]; then
  PORT_NAME="/dev/ttyUSB0"
fi

echo "Port: $PORT_NAME"

git checkout master && \
git pull origin master && \
sh inject-version.sh && \
sh build-uno.sh && \
sh upload-uno.sh $PORT_NAME && \

# Clean avoid git merge conflicts
sh clean.sh && \

echo "Pull, build, and upload complete."

