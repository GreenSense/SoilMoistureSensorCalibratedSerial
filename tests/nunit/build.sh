echo "Starting build for project"
echo "Dir: $PWD"

DIR=$PWD

msbuild src/SoilMoistureSensorCalibratedSerial.sln /p:Configuration=Release
