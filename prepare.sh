#!/bin/bash

# Note: You may need to run this script with sudo

echo "Preparing for SoilMoistureSensorCalibratedSerial project"

DIR=$PWD

#git submodule update --init --recursive

sudo apt-get update

# curl
if ! type "curl" > /dev/null; then
  sudo apt-get install -y curl
fi

# git
if ! type "git" > /dev/null; then
  sudo apt-get install -y git
fi

# python
if ! type "python" > /dev/null; then
  sudo apt-get install -y python python-pip
fi

# platform.io
if ! type "pio" > /dev/null; then
  #sudo python -c "$(curl -fsSL https://raw.githubusercontent.com/platformio/platformio/master/scripts/get-platformio.py)"
  sudo python -c "$(curl -fsSL https://raw.githubusercontent.com/platformio/platformio/develop/scripts/get-platformio.py)"
fi

# mono
if ! type "msbuild" > /dev/null; then
  sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
  sudo echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list

  sudo apt-get update && apt-get install -y mono-devel mono-complete ca-certificates-mono msbuild
fi

cd tests/nunit && \
  sh prepare.sh && \
cd $DIR
