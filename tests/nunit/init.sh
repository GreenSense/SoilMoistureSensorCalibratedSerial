DIR=$PWD

git clone http://github.com/CompulsiveCoder/duinocom.git lib/duinocom
cd lib/duinocom
sh init.sh &&
sh build.sh &&

cd $DIR
