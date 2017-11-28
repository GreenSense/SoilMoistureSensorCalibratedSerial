echo "Getting library files"
echo "  Dir: $PWD"

LIB_DIR=$PWD

NUGET_FILE="nuget.exe"

cert-sync --quiet /etc/ssl/certs/ca-certificates.crt

if [ ! -f "$NUGET_FILE" ];
then
    wget http://nuget.org/nuget.exe
    mono nuget.exe update -self
fi

if [ ! -d "NUnit.2.6.4" ]; then
    mono nuget.exe install nunit -version 2.6.4
fi

if [ ! -d "NUnit.Runners.2.6.4" ]; then
    mono nuget.exe install nunit.runners -version 2.6.4
fi

if [ ! -d "ArduinoSerialControllerClient.1.0.3" ]; then
    mono nuget.exe install ArduinoSerialControllerClient -version 1.0.3
fi
