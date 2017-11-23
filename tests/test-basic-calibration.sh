#!/bin/sh

echo "----------------------------------------"
echo "Starting basic calibration test"

echo "Sending 255"

./lib/arduino-serial/arduino-serial --baud=9600 --port=/dev/ttyUSB1 --sendline="255" &&

OUT=$(./lib/arduino-serial/arduino-serial --baud=9600 --port=/dev/ttyUSB0 -q -r) &&

echo $OUT &&

for pair in $(echo $OUT | sed "s/;/ /g")
do

  var1=$(echo $pair | cut -f1 -d:)
  var2=$(echo $pair | cut -f2 -d:)

  if [[ $var1 = "C" ]]
  then
      echo "Calibrated: $var2"

      if [ "$var2" -gt 1 ];then
        echo "Invalid result";
        exit 1;
      fi
  fi

  if [[ $var1 = "R" ]]
  then
      echo "Raw: $var2"


      if [ "$var2" -lt 1023 ];then
        echo "Invalid result";
        exit 1;
      fi

  fi

done

if [ $? -eq 0 ]; then
  echo "Test section completed!"
  echo ""
else
  echo "Test section failed"
  exit 1
fi

echo "Sending 0"

./lib/arduino-serial/arduino-serial --baud=9600 --port=/dev/ttyUSB1 --sendline="0" &&

OUT=$(./lib/arduino-serial/arduino-serial --baud=9600 --port=/dev/ttyUSB0 -q -r) &&

echo $OUT


for pair in $(echo $OUT | sed "s/;/ /g")
do

  var1=$(echo $pair | cut -f1 -d:)
  var2=$(echo $pair | cut -f2 -d:)

  if [[ $var1 = "C" ]]
  then
      echo "Calibrated: $var2"

      if [ "$var2" -lt 99 ];then
        echo "Invalid result";
        exit 1;
      fi
  fi

  if [[ $var1 = "R" ]]
  then
      echo "Raw: $var2"

      if [ "$var2" -gt 6 ];then
        echo "Invalid result";
        exit 1;
      fi
  fi

done

if [ $? -eq 0 ]; then
  echo "Test section completed!"
else
  echo "Test section failed"
  exit 1
fi

echo ""

if [ $? -eq 0 ]; then
  echo "Test completed successfully!"
  echo "----------------------------------------"
else
  echo "Test failed"
  exit 1
fi