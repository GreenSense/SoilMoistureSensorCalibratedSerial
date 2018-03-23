#!/bin/bash

. ./check-ci-skip.sh

if [ $SKIP_CI = 0 ]; then

  BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

  if [ "$BRANCH" = "dev" ];  then
  else
    echo "Skipping commit version. Only pushed for 'dev' branch not '$BRANCH'"
  fi
else
  echo "Skipping commit version [ci skip]"
fi
