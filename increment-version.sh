#!/bin/sh

echo "Incrementing version"

CURRENT_VERSION=$(cat buildnumber.txt)

echo "Current: $CURRENT_VERSION"

CURRENT_VERSION=$(($CURRENT_VERSION + 1))

echo "New version: $CURRENT_VERSION"

echo $CURRENT_VERSION > buildnumber.txt