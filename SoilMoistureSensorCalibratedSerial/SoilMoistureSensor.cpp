#include <Arduino.h>

#include <EEPROM.h>

#include "Common.h"
#include "SoilMoistureSensor.h"

#define soilMoistureSensorPin A0
#define soilMoistureSensorPowerPin 12

bool soilMoistureSensorIsOn = true;
long lastSensorOnTime = 0;
int delayAfterTurningSensorOn = 3 * 1000;

int soilMoistureSensorIsCalibratedFlagAddress = 0;
int drySoilMoistureCalibrationValueAddress = 2;
int wetSoilMoistureCalibrationValueAddress = 3;

bool reverseSoilMoistureSensor = false;

long lastSoilMoistureSensorReadingTime;
long soilMoistureSensorReadingInterval = 3 * 1000;

int soilMoistureLevelCalibrated = 0;
int soilMoistureLevelRaw = 0;

int drySoilMoistureCalibrationValue = 0;
int wetSoilMoistureCalibrationValue = 0;

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
  drySoilMoistureCalibrationValue = (reverseSoilMoistureSensor ? 0 : 1024);
  wetSoilMoistureCalibrationValue = (reverseSoilMoistureSensor ? 1024 : 0);
}

void setDrySoilMoistureCalibrationValue(int drySoilMoistureCalibrationValue)
{
  if (isDebugMode)
  {
    Serial.print("Setting dry calibration value to EEPROM: ");
    Serial.println(drySoilMoistureCalibrationValue);
  }
  
  int compactValue = drySoilMoistureCalibrationValue / 4;

  EEPROM.write(drySoilMoistureCalibrationValueAddress, compactValue); // Must divide by 4 to make it fit in eeprom

  setEEPROMIsCalibratedFlag();
}

void setWetSoilMoistureCalibrationValue(int wetSoilMoistureCalibrationValue)
{
  if (isDebugMode)
  {
    Serial.print("Setting wet calibration value to EEPROM: ");
    Serial.println(wetSoilMoistureCalibrationValue);
  }

  int compactValue = wetSoilMoistureCalibrationValue / 4;

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
