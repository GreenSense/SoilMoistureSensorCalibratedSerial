#!/bin/bash

DIR=$PWD

pio run && \

cd tests/nunit && \
sh build.sh && \

cd $DIR

