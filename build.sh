#!/bin/bash

cd tests/nunit && \
sh build.sh && \

pio run
