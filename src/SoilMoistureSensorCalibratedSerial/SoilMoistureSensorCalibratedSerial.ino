#include <Arduino.h>
#include <EEPROM.h>

#include "Common.h"
#include "SoilMoistureSensor.h"

long lastSerialOutputTime = 0;
long serialOutputInterval = soilMoistureSensorReadingInterval;
//long serialOutputInterval = 1;

#define SERIAL_MODE_CALIBRATED 1
#define SERIAL_MODE_RAW 2
#define SERIAL_MODE_CSV 3
#define SERIAL_MODE_QUERYSTRING 4

int serialMode = SERIAL_MODE_CSV;

int loopNumber = 0;

void setup()
{
  
  Serial.begin(9600);
  //Serial.begin(115200);

  if (isDebugMode)
    Serial.println("Starting soil moisture sensor");

  setupSoilMoistureSensor();
}

void loop()
{
  loopNumber++;

  if (isDebugMode)
  {
    Serial.println("==============================");
    Serial.print("===== Start Loop: ");
    Serial.println(loopNumber);
    Serial.println("==============================");
  }

  checkCommand();

  takeSoilMoistureSensorReading();

  serialPrintData();

  if (isDebugMode)
  {
    Serial.println("==============================");
    Serial.print("===== End Loop: ");
    Serial.println(loopNumber);
    Serial.println("==============================");
    Serial.println("");
    Serial.println("");
  }

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
        setDrySoilMoistureCalibrationValueToCurrent();
        break;
      case 'W':
        setWetSoilMoistureCalibrationValueToCurrent();
        break;
      case 'X':
        restoreDefaultSettings();
        break;
      case 'R':
        reverseSoilMoistureCalibrationValues();
        break;
      case 'Z':
        Serial.println("Toggling isDebugMode");
        isDebugMode = !isDebugMode;
        break;
    }
  }
}

/* Settings */
void restoreDefaultSettings()
{
  Serial.println("Restoring default settings");

  restoreDefaultCalibrationSettings();
}

/* Serial Output */
void serialPrintData()
{
  bool isTimeToPrintData = lastSerialOutputTime + serialOutputInterval < millis()
      || lastSerialOutputTime == 0;

  bool isReadyToPrintData = isTimeToPrintData && soilMoistureSensorReadingHasBeenTaken;

  if (isReadyToPrintData)
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
  }
}
