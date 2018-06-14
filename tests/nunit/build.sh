echo "Starting build for project"
echo "Dir: $PWD"

DIR=$PWD

xbuild src/SoilMoistureSensorCalibratedSerial.sln /p:Configuration=Release
