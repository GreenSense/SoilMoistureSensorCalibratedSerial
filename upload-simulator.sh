# May need be run as sudo

SIMULATOR_TMP_DIR="_simulatortmp"

# Remove old versions
rm -rf $SIMULATOR_TMP_DIR

# Make a new directory
mkdir -p $SIMULATOR_TMP_DIR
cd $SIMULATOR_TMP_DIR

# Clone the latest version
git clone https://github.com/CompulsiveCoder/SerialToPWM.git

cd SerialToPWM

sudo sh upload-to-port.sh "/dev/ttyUSB1"

rm -rf $SIMULATOR_TMP_DIR
