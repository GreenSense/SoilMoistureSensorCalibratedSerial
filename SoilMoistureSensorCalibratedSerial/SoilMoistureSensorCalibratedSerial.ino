#include <Arduino.h>
#include <EEPROM.h>

extern bool isDebugMode = false;


long lastSerialOutputTime = 0;
long serialOutputInterval = 3 * 1000;//soilMoistureSensorReadingInterval;

/*#define SERIAL_MODE_CALIBRATED 1
#define SERIAL_MODE_RAW 2
#define SERIAL_MODE_CSV 3
#define SERIAL_MODE_QUERYSTRING 4

int serialMode = SERIAL_MODE_CSV;*/


#include "SoilMoistureSensor.h"

void setup()
{
  
  Serial.begin(9600);
  //Serial.begin(115200);

  //if (isDebug)
  //  Serial.println("Starting soil moisture sensor");

  setupSoilMoistureSensor();
}

void loop()
{
  checkCommand();

  takeSoilMoistureSensorReading();

  serialPrintData();

  delay(1);
}

/* Commands */
void checkCommand()
{
  while (Serial.available() > 0)
  {
    char command = Serial.read();

    Serial.println(command);

    switch (command)
    {
      case 'D':
       // lastSoilMoistureSensorReadingTime = 0;
     //   takeSoilMoistureSensorReading();
    //    setDrySoilMoistureCalibrationValue(soilMoistureLevelRaw);
        break;
    /*  case 'W':
        lastSoilMoistureSensorReadingTime = 0;
        takeSoilMoistureSensorReading();
        setWetSoilMoistureCalibrationValue(soilMoistureLevelRaw);
        break;
      case 'Z':
        Serial.println("Toggling IsDebug");
        isDebugMode = !isDebugMode;
        break;*/
    }
  }
}


/* Serial Output */
void serialPrintData()
{
  /*if (lastSerialOutputTime + serialOutputInterval < millis()
      || lastSerialOutputTime == 0)
  {
	long numberOfSecondsOnline = millis()/1000;

    if (serialMode == SERIAL_MODE_CSV)
    {
      Serial.print("D;"); // This prefix indicates that the line contains data.
      Serial.print("T:");
      Serial.print(numberOfSecondsOnline);
      Serial.print(";");
      Serial.print("R:");
      Serial.print(soilMoistureLevelRaw);
      Serial.print(";");
      Serial.print("C:");
      Serial.print(soilMoistureLevelCalibrated);
      Serial.print(";");
      Serial.print("D:");
      Serial.print(drySoilMoistureCalibrationValue);
      Serial.print(";");
      Serial.print("W:");
      Serial.print(wetSoilMoistureCalibrationValue);
      Serial.print(";");
      Serial.println();
    }
    else if (serialMode == SERIAL_MODE_QUERYSTRING)
    {
      Serial.print("time=");
      Serial.print(numberOfSecondsOnline);
      Serial.print("&");
      Serial.print("raw=");
      Serial.print(soilMoistureLevelRaw);
      Serial.print("&");
      Serial.print("calibrated=");
      Serial.print(soilMoistureLevelCalibrated);
      Serial.print("&");
      Serial.print("dry=");
      Serial.print(drySoilMoistureCalibrationValue);
      Serial.print("&");
      Serial.print("wet=");
      Serial.print(wetSoilMoistureCalibrationValue);
      Serial.println();
    }
	else if (serialMode == SERIAL_MODE_CALIBRATED)
	{
      Serial.println(soilMoistureLevelCalibrated);
	}
	else if (serialMode == SERIAL_MODE_RAW)
	{
      Serial.println(soilMoistureLevelRaw);
	}

    lastSerialOutputTime = millis();
  }*/
}