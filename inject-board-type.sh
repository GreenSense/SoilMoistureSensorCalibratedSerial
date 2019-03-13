BOARD_TYPE=$1

if [ ! $BOARD_TYPE ]; then
  echo "Provide a board type as an argument. For example 'nano' or 'uno'."
  exit 1
fi

SOURCE_FILE="src/SoilMoistureSensorCalibratedSerial/SoilMoistureSensorCalibratedSerial.ino"

sed -i "s/#define BOARD_TYPE .*/#define BOARD_TYPE \"$BOARD_TYPE\"/" $SOURCE_FILE
