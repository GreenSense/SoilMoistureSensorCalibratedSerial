#!/bin/bash

DIR=$PWD

# Note: You may need to run this script with sudo

git submodule update --init

sudo apt-get update

# python
if ! type "python" > /dev/null; then
  sudo apt-get install -y python python-pip
fi

# platform.io
if ! type "pio" > /dev/null; then
  #sudo python -c "$(curl -fsSL https://raw.githubusercontent.com/platformio/platformio/master/scripts/get-platformio.py)"
  sudo python -c "$(curl -fsSL https://raw.githubusercontent.com/platformio/platformio/develop/scripts/get-platformio.py)"
fi

cd tests/nunit/lib/ArduinoSerialControllerClient
sudo sh prepare.sh
cd $DIR
