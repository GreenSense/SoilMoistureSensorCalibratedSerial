
docker run -i -v /var/run/docker.sock:/var/run/docker.sock -v $PWD:/SoilMoistureSensorCalibratedSerial compulsivecoder/ubuntu-arm-iot-mono /bin/bash -c "rsync -avzh /SoilMoistureSensorCalibratedSerial/ /project && cd /project && sh prepare.sh && sh init.sh && sh build-all.sh"
