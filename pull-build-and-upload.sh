#!/bin/bash
#BRANCH=$(git branch | sed -n -e 's/^\* \(.*\)/\1/p')

git checkout master && \
git pull origin master && \

sh build-and-upload.sh
