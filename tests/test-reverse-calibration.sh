#!/bin/sh

echo "----------------------------------------"
echo "Starting reverse calibration test"

echo "Reversing calibration"

./lib/arduino-serial/arduino-serial --baud=9600 --port=/dev/ttyUSB0 --sendline="R"

echo "Sending 0"

./lib/arduino-serial/arduino-serial --baud=9600 --port=/dev/ttyUSB1 --sendline="0"

OUT=$(./lib/arduino-serial/arduino-serial --baud=9600 --port=/dev/ttyUSB0 -q -r)

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


      if [ "$var2" -gt 2 ];then
        echo "Invalid result";
        exit 1;
      fi

  fi

done

echo "Test section completed!"
echo ""

echo "Sending 255"

./lib/arduino-serial/arduino-serial --baud=9600 --port=/dev/ttyUSB1 --sendline="255"

OUT=$(./lib/arduino-serial/arduino-serial --baud=9600 --port=/dev/ttyUSB0 -q -r)

echo $OUT

for pair in $(echo $OUT | sed "s/;/ /g")
do

  var1=$(echo $pair | cut -f1 -d:)
  var2=$(echo $pair | cut -f2 -d:)

  #echo "Key: $var1"
  #echo "Value: $var2"

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

echo "Test section completed!"
echo ""

echo "Test completed successfully!"
echo "----------------------------------------"
