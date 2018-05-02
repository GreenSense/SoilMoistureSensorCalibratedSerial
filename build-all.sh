
. ./check-ci-skip.sh

if [ $SKIP_CI = 1 ]; then
  echo "Skipping build [ci skip]"
else
  sh build.sh && \
  sh build-tests.sh
fi