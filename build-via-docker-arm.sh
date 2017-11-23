
docker run -i -v /var/run/docker.sock:/var/run/docker.sock -v $PWD:/projectspace resin/rpi-raspbian /bin/bash -c "cd /projectspace && sh prepare.sh && sh init.sh && sh build.sh"
