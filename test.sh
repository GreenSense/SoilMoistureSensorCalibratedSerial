
. ./check-ci-skip.sh

DIR=$PWD

if [ $SKIP_CI = 1 ]; then
  echo "Skipping test [ci skip]"
else
    cd tests/nunit/
    sh build-and-test-all.sh && \
    cd $DIR
fi
