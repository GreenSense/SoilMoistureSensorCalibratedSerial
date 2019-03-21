echo "Injecting version into sketch..."

VERSION_ARGUMENT=$1

if [ ! $VERSION_ARGUMENT ]; then
  VERSION=$(cat version.txt)
  BUILD=$(cat buildnumber.txt)

  FULL_VERSION="$VERSION-$BUILD"
else
  FULL_VERSION=$VERSION_ARGUMENT
fi

echo "Version: $FULL_VERSION"

SOURCE_FILE="src/SoilMoistureSensorCalibratedSerial/SoilMoistureSensorCalibratedSerial.ino"

echo "Source file:"
echo "$SOURCE_FILE"

sed -i "s/#define VERSION .*/#define VERSION \"$FULL_VERSION\"/" $SOURCE_FILE || exit 1

echo "Done"
