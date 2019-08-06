PORT_NAME=$1

if [ ! $PORT_NAME ]; then
  PORT_NAME="/dev/ttyUSB0"
fi

echo "Port: $PORT_NAME"

sh inject-board-type.sh "nano" && \

pio run --target upload --environment=nanoatmega328 --upload-port=$PORT_NAME || exit 1

echo ""
echo ""
echo "Upload complete"
