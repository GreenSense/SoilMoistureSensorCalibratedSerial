#!/bin/bash

. ./garden-credentials.sh

sshpass -p $GARDEN_PASSWORD ssh $GARDEN_USER:$GARDEN_PASSWORD@$GARDEN_HOSTNAME 'cd workspace/GreenSense/Index/sketches/monitor/SoilMoistureSensorCalibratedSerial && sh pull-build-and-upload.sh'


