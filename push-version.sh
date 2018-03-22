#!/bin/bash

. ./check-ci-skip.sh

if [ ! $SKIP_CI ]; then
    BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

    git commit buildnumber.txt -m "Updated version [ci skip]" && \
    git push origin $BRANCH
else
    echo "Skipping push version [ci skip]"
fi