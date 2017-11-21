#include <Arduino.h>

#include <EEPROM.h>

#include "Common.h"
#include "SoilMoistureSensor.h"

#define soilMoistureSensorPin A0
#define soilMoistureSensorPowerPin 12

bool soilMoistureSensorIsOn = true;
long lastSensorOnTime = 0;
int delayAfterTurningSensorOn = 3 * 1000;

long lastSoilMoistureSensorReadingTime = 0;
long soilMoistureSensorReadingInterval = 3 * 1000;

int soilMoistureLevelCalibrated = 0;
int soilMoistureLevelRaw = 0;

bool reverseSoilMoistureSensor = false;
//int drySoilMoistureCalibrationValue = 1024;
int drySoilMoistureCalibrationValue = (reverseSoilMoistureSensor ? 0 : 1024);
//int wetSoilMoistureCalibrationValue = 0;
int wetSoilMoistureCalibrationValue = (reverseSoilMoistureSensor ? 1024 : 0);

int soilMoistureSensorIsCalibratedFlagAddress = 0;
int drySoilMoistureCalibrationValueAddress = 2;
int wetSoilMoistureCalibrationValueAddress = 3;

/* Setup */
void setupSoilMoistureSensor()
{
  setupCalibrationValues();

  pinMode(soilMoistureSensorPowerPin, OUTPUT);

  // If the interval is less than specified delay then turn the sensor on and leave it on (otherwise it will be turned on each time it's needed)
  if (soilMoistureSensorReadingInterval <= delayAfterTurningSensorOn)
  {
    turnSoilMoistureSensorOn();
  }
}

/* Sensor On/Off */
void turnSoilMoistureSensorOn()
{
  if (isDebugMode)
    Serial.println("Turning sensor on");

  digitalWrite(soilMoistureSensorPowerPin, HIGH);

  lastSensorOnTime = millis();

  soilMoistureSensorIsOn = true;
  
}

void turnSoilMoistureSensorOff()
{
  if (isDebugMode)
    Serial.println("Turning sensor off");

  digitalWrite(soilMoistureSensorPowerPin, LOW);

  soilMoistureSensorIsOn = false;
}

/* Sensor Readings */
void takeSoilMoistureSensorReading()
{
  bool sensorReadingIsDue = lastSoilMoistureSensorReadingTime + soilMoistureSensorReadingInterval < millis() || lastSoilMoistureSensorReadingTime == 0;

  if (sensorReadingIsDue)
  {
    if (isDebugMode)
      Serial.println("Sensor reading is due");

  	bool sensorGetsTurnedOff = soilMoistureSensorReadingInterval > delayAfterTurningSensorOn;
  
  	bool sensorIsOffAndNeedsToBeTurnedOn = !soilMoistureSensorIsOn && sensorGetsTurnedOff;
  
  	bool postSensorOnDelayHasPast = lastSensorOnTime + delayAfterTurningSensorOn < millis();
  
  	bool soilMoistureSensorIsOnAndReady = soilMoistureSensorIsOn && (postSensorOnDelayHasPast || !sensorGetsTurnedOff);

    bool soilMoistureSensorIsOnButSettling = soilMoistureSensorIsOn && !postSensorOnDelayHasPast && !sensorGetsTurnedOff;

    if (isDebugMode)
    {
        Serial.print("Sensor gets turned off: ");
        Serial.println(sensorGetsTurnedOff);
        Serial.print("Sensor is off and needs to be turned on: ");
        Serial.println(sensorIsOffAndNeedsToBeTurnedOn);
        Serial.print("Post sensor on delay has past: ");
        Serial.println(postSensorOnDelayHasPast);
        Serial.print("Sensor is on and ready: ");
        Serial.println(soilMoistureSensorIsOnAndReady);
        Serial.print("Sensor is on but settling: ");
        Serial.println(soilMoistureSensorIsOnButSettling);
    }

    if (sensorIsOffAndNeedsToBeTurnedOn)
    {
      turnSoilMoistureSensorOn();
    }
    else if (soilMoistureSensorIsOnButSettling)
    {
      // Skip this loop. Wait for the sensor to settle in before taking a reading.
      if (isDebugMode)
        Serial.println("Soil moisture sensor is settling after being turned on");
    }
    else if (soilMoistureSensorIsOnAndReady)
    {
      if (isDebugMode)
        Serial.println("Preparing to take reading");

      lastSoilMoistureSensorReadingTime = millis();

      soilMoistureLevelRaw = getAverageSoilMoistureSensorReading();

      soilMoistureLevelCalibrated = calculateSoilMoistureLevel(soilMoistureLevelRaw);

      if (soilMoistureLevelCalibrated < 0)
        soilMoistureLevelCalibrated = 0;

      if (soilMoistureLevelCalibrated > 100)
        soilMoistureLevelCalibrated = 100;

      // If the interval is less than 2 seconds then don't turn the sensor off
      if (soilMoistureSensorReadingInterval > delayAfterTurningSensorOn)
      {
        turnSoilMoistureSensorOff();
      }
    }
  }
}

double getAverageSoilMoistureSensorReading()
{
  int readingSum  = 0;
  int totalReadings = 10;

  for (int i = 0; i < totalReadings; i++)
  {
    int reading = analogRead(soilMoistureSensorPin);

    readingSum += reading;
  }

  double averageReading = readingSum / totalReadings;

  return averageReading;
}

double calculateSoilMoistureLevel(int soilMoistureSensorReading)
{
  return map(soilMoistureSensorReading, drySoilMoistureCalibrationValue, wetSoilMoistureCalibrationValue, 0, 100);
}


/* Calibration */
void setupCalibrationValues()
{
  bool eepromIsSet = EEPROM.read(soilMoistureSensorIsCalibratedFlagAddress) == 99;

  if (eepromIsSet)
  {
    if (isDebugMode)
    	Serial.println("EEPROM calibration values have been set. Loading.");

    drySoilMoistureCalibrationValue = getDrySoilMoistureCalibrationValue();
    wetSoilMoistureCalibrationValue = getWetSoilMoistureCalibrationValue();
  }
  else
  {
    if (isDebugMode)
      Serial.println("EEPROM calibration values have not been set. Using defaults.");
    
    setDrySoilMoistureCalibrationValue(drySoilMoistureCalibrationValue);
    setWetSoilMoistureCalibrationValue(wetSoilMoistureCalibrationValue);
  }
}

void setDrySoilMoistureCalibrationValueToCurrent()
{
  lastSoilMoistureSensorReadingTime = 0;
  takeSoilMoistureSensorReading();
  setDrySoilMoistureCalibrationValue(soilMoistureLevelRaw);
}

void setDrySoilMoistureCalibrationValue(int newValue)
{
  Serial.print("Setting dry soil moisture sensor calibration value: ");
  Serial.println(newValue);

  drySoilMoistureCalibrationValue = newValue;
  
  int compactValue = newValue / 4;

  EEPROM.write(drySoilMoistureCalibrationValueAddress, compactValue); // Must divide by 4 to make it fit in eeprom

  setEEPROMIsCalibratedFlag();
}

void setWetSoilMoistureCalibrationValueToCurrent()
{
  lastSoilMoistureSensorReadingTime = 0;
  takeSoilMoistureSensorReading();
  setWetSoilMoistureCalibrationValue(soilMoistureLevelRaw);
}

void setWetSoilMoistureCalibrationValue(int newValue)
{
  Serial.print("Setting wet soil moisture sensor calibration value: ");
  Serial.println(newValue);

  wetSoilMoistureCalibrationValue = newValue;

  int compactValue = newValue / 4;

  EEPROM.write(wetSoilMoistureCalibrationValueAddress, compactValue); // Must divide by 4 to make it fit in eeprom
  
  setEEPROMIsCalibratedFlag();
}

int getDrySoilMoistureCalibrationValue()
{
  int value = EEPROM.read(drySoilMoistureCalibrationValueAddress);

  if (value == 0
      || value == 255)
    return drySoilMoistureCalibrationValue;
  else
  {
    int drySoilMoistureSensorValue = value * 4; // Must multiply by 4 to get the original value
  
    if (isDebugMode)
    {
      Serial.print("Dry calibration value found in EEPROM: ");
      Serial.println(drySoilMoistureSensorValue);
    }

    return drySoilMoistureSensorValue;
  }
}

int getWetSoilMoistureCalibrationValue()
{
  int value = EEPROM.read(wetSoilMoistureCalibrationValueAddress);

  int wetSoilMoistureSensorValue = value * 4; // Must multiply by 4 to get the original value

  if (isDebugMode)
  {
    Serial.print("Wet calibration value found in EEPROM: ");
    Serial.println(wetSoilMoistureSensorValue);
  }

  return wetSoilMoistureSensorValue;
}

void setEEPROMIsCalibratedFlag()
{
  if (EEPROM.read(soilMoistureSensorIsCalibratedFlagAddress) != 99)
    EEPROM.write(soilMoistureSensorIsCalibratedFlagAddress, 99);
}

void removeEEPROMIsCalibratedFlag()
{
    EEPROM.write(soilMoistureSensorIsCalibratedFlagAddress, 0);
}

void restoreDefaultCalibrationSettings()
{
  removeEEPROMIsCalibratedFlag();

  drySoilMoistureCalibrationValue = (reverseSoilMoistureSensor ? 0 : 1024);
  wetSoilMoistureCalibrationValue = (reverseSoilMoistureSensor ? 1024 : 0);

  setDrySoilMoistureCalibrationValue(drySoilMoistureCalibrationValue);
  setWetSoilMoistureCalibrationValue(wetSoilMoistureCalibrationValue);
}
