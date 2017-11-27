echo "Getting library files"
echo "  Dir: $PWD"

LIB_DIR=$PWD

NUGET_FILE="nuget.exe"

if [ ! -f "$NUGET_FILE" ];
then
    wget http://nuget.org/nuget.exe
fi

mono nuget.exe install nunit -version 2.6.4
mono nuget.exe install nunit.runners -version 2.6.4

git clone http://gitub.com/CompulsiveCoder/duinocom.git
cd duinocom
sh init.sh &&
sh build.sh
