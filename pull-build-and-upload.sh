#!/bin/bash
git checkout master && \
git pull origin master && \

sh build-and-upload.sh
