DIR=$PWD
cd tests/lib
sh get-libs.sh &&
cd $DIR

cd tests/nunit
sh init.sh &&
cd $DIR
