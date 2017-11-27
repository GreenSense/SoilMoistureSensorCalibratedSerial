# May need be run as sudo

# Specify a temporary directory name
SIMULATOR_TMP_DIR="_simulatortmp"

# Remove old versions
rm -rf $SIMULATOR_TMP_DIR

# Make a new directory
mkdir -p $SIMULATOR_TMP_DIR
cd $SIMULATOR_TMP_DIR

# Clone the latest version
git clone https://github.com/CompulsiveCoder/SerialToPWM.git

cd SerialToPWM

# Upload to USB1
sudo sh upload-to-port.sh "/dev/ttyUSB1"

# Remove the temporary directory
rm -rf $SIMULATOR_TMP_DIR
