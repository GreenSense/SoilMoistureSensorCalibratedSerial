docker run -it --rm -v $PWD:/project-src -v /var/run/docker.sock:/var/run/docker.sock compulsivecoder/ubuntu-arm-iot-mono /bin/bash -c "rsync -av --exclude='.git' /project-src/* /project-dest && cd /project-dest && sh build-all.sh"

#docker run -i -rm -v /var/run/docker.sock:/var/run/docker.sock -v $PWD:/SoilMoistureSensorCalibratedSerial compulsivecoder/ubuntu-arm-iot-mono /bin/bash -c "rsync -avzh /SoilMoistureSensorCalibratedSerial/ /project && cd /project && sh prepare.sh && sh init.sh && sh build-all.sh"
