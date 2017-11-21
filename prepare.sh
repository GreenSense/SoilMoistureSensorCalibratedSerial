# Note: You may need to run this script with sudo

sudo apt update &&
sudo apt -y install curl &&
sudo python -c "$(curl -fsSL https://raw.githubusercontent.com/platformio/platformio/develop/scripts/get-platformio.py)"

# Or

#pip install -U platformio
