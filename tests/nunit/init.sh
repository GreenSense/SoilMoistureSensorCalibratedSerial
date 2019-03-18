echo "Initializing nunit tests for project"

DIR=$PWD

cd lib && \
sh get-libs.sh && \
cd $DIR
