# May need be run as sudo

echo ""
echo "Uploading simulator sketch"
echo ""

# Specify a temporary directory name
SIMULATOR_TMP_DIR="_simulatortmp"

# Remove old versions
rm -rf $SIMULATOR_TMP_DIR

# Make a new directory
mkdir -p $SIMULATOR_TMP_DIR
cd $SIMULATOR_TMP_DIR

echo "Preparing to clone ArduinoSerialController"
echo "Dir:"
echo "  $PWD"

# Clone the latest version
git clone https://github.com/CompulsiveCoder/ArduinoSerialController.git

cd ArduinoSerialController

# Upload to USB1
sh upload-to-port.sh "/dev/ttyUSB1"

# Remove the temporary directory
rm -rf $SIMULATOR_TMP_DIR
