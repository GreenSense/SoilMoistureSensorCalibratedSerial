VERSION=$(cat version.txt)
BUILD_NUMBER=$(cat buildnumber.txt)

FULL_VERSION="$VERSION.$BUILD_NUMBER"

SOURCE_FILE="src/SoilMoistureSensorCalibratedSerial/SoilMoistureSensorCalibratedSerial.ino"

sed -i "s/String version = .*/String version = \"$FULL_VERSION\"/" $SOURCE_FILE
