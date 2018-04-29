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
sh build-and-upload.sh $PORT_NAME && \

# Revert the sketch file to avoid git merge conflicts
git checkout src/SoilMoistureSensorCalibratedSerial/SoilMoistureSensorCalibratedSerial.ino && \

echo "Pull, build, and upload complete."

