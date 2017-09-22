#include <Arduino.h>
#include <EEPROM.h>

long second = 1000;
long minute = second * 60;
long hour = minute * 60;

long lastReadingTime = 0;

//long readingInterval = 1 * hour;
//long readingInterval = 5 * minute;
long readingInterval = 3 * second;

#define moisturePin 0
#define sensorPowerPin 12

int moistureLevel = 0;
int moistureLevelRaw = 0;

bool reverseSoilMoistureSensor = false;
int dryReading = (reverseSoilMoistureSensor ? 0 : 1024);
int wetReading = (reverseSoilMoistureSensor ? 1024 : 0);

int flagAddress = 0;
int dryReadingAddress = 2;
int wetReadingAddress = 3;

bool sensorIsOn = true;
long lastSensorOnTime = 0;
int delayAfterTurningSensorOn = 3 * second;

long lastSerialOutputTime = 0;
long serialOutputInterval = readingInterval;

bool isDebug = false;

#define SERIAL_MODE_CALIBRATED 1
#define SERIAL_MODE_RAW 2
#define SERIAL_MODE_CSV 3
#define SERIAL_MODE_QUERYSTRING 4

int serialMode = SERIAL_MODE_CSV;

void setup()
{
  Serial.begin(9600);

  if (isDebug)
    Serial.println("Starting soil moisture sensor");

  pinMode(sensorPowerPin, OUTPUT);

  // If the interval is less than 2 seconds turn the sensor on and leave it on (otherwise it will be turned on just before it's needed)
  if (readingInterval <= delayAfterTurningSensorOn)
  {
    sensorOn();

    delay(delayAfterTurningSensorOn);
  }

}

void loop()
{
  checkCommand();

  takeReading();

  serialPrintData();

  delay(1);
}

/* Commands */
void checkCommand()
{
  // TODO: Is this function still required now that calibration is completed using buttons?
  while (Serial.available() > 0)
  {
    char command = Serial.read();

    Serial.println(command);

    switch (command)
    {
      case 'D':
        lastReadingTime = 0;
        takeReading();
        setDryReading(moistureLevelRaw);
        break;
      case 'W':
        lastReadingTime = 0;
        takeReading();
        setWetReading(moistureLevelRaw);
        break;
      case 'Z':
        Serial.println("Toggling IsDebug");
        isDebug = !isDebug;
        break;
    }
  }
}

/* Readings */
void takeReading()
{
  if (lastReadingTime + readingInterval < millis()
      || lastReadingTime == 0)
  {
    if (!sensorIsOn && readingInterval > delayAfterTurningSensorOn)
    {
      sensorOn();
    }
    else if (sensorIsOn && lastSensorOnTime + delayAfterTurningSensorOn < millis()
             || readingInterval < delayAfterTurningSensorOn)
    {
      if (isDebug)
        Serial.println("Preparing to take reading");

      lastReadingTime = millis();

      moistureLevelRaw = getAverageReading();

      moistureLevel = calculateMoistureLevel(moistureLevelRaw);

      if (moistureLevel < 0)
        moistureLevel = 0;

      if (moistureLevel > 100)
        moistureLevel = 100;

      // If the interval is less than 2 seconds then don't turn the sensor off
      if (readingInterval > delayAfterTurningSensorOn)
      {
        sensorOff();
      }
    }
  }
}

double getAverageReading()
{
  int readingSum  = 0;
  int totalReadings = 10;

  for (int i = 0; i < totalReadings; i++)
  {
    int reading = analogRead(moisturePin);

    readingSum += reading;
  }

  double averageReading = readingSum / totalReadings;

  return averageReading;
}

int calculateMoistureLevel(int reading)
{
  return map(reading, dryReading, wetReading, 0, 100);
}

/* Serial Output */
void serialPrintData()
{
  if (lastSerialOutputTime + serialOutputInterval < millis()
      || lastSerialOutputTime == 0)
  {
    if (serialMode == SERIAL_MODE_CSV)
    {
      Serial.print("D;"); // This prefix indicates that the line contains data.
      Serial.print("Raw:");
      Serial.print(moistureLevelRaw);
      Serial.print(";");
      Serial.print("Calibrated:");
      Serial.print(moistureLevel);
      Serial.print(";");
      Serial.print("Dry:");
      Serial.print(dryReading);
      Serial.print(";");
      Serial.print("Wet:");
      Serial.print(wetReading);
      Serial.print(";");
      Serial.println();
    }
    else if (serialMode == SERIAL_MODE_QUERYSTRING)
    {
      Serial.print("raw=");
      Serial.print(moistureLevelRaw);
      Serial.print("&");
      Serial.print("calibrated=");
      Serial.print(moistureLevel);
      Serial.print("&");
      Serial.print("dry=");
      Serial.print(dryReading);
      Serial.print("&");
      Serial.print("wet=");
      Serial.print(wetReading);
      Serial.println();
    }
	else if (serialMode == SERIAL_MODE_CALIBRATED)
	{
      Serial.println(moistureLevel);
	}
	else if (serialMode == SERIAL_MODE_RAW)
	{
      Serial.println(moistureLevelRaw);
	}

    lastSerialOutputTime = millis();
  }
}

/* Sensor */
void sensorOn()
{
  if (isDebug)
    Serial.println("Turning sensor on");

  digitalWrite(sensorPowerPin, HIGH);

  lastSensorOnTime = millis();

  delay(delayAfterTurningSensorOn);

  sensorIsOn = true;
}

void sensorOff()
{
  if (isDebug)
    Serial.println("Turning sensor off");

  digitalWrite(sensorPowerPin, LOW);

  sensorIsOn = false;
}

/* Calibration */
void setDryReading(int reading)
{
  dryReading = reading;

  if (isDebug)
  {
    Serial.print("Setting dry reading to EEPROM: ");
    Serial.println(reading);
  }

  int compactValue = reading / 4;

  EEPROM.write(dryReadingAddress, compactValue); // Must divide by 4 to make it fit in eeprom

  setEEPROMFlag();
}

void setWetReading(int reading)
{
  wetReading = reading;

  if (isDebug)
  {
    Serial.print("Setting wet reading to EEPROM: ");
    Serial.println(reading);
  }

  int compactValue = reading / 4;

  EEPROM.write(wetReadingAddress, compactValue); // Must divide by 4 to make it fit in eeprom
  
  setEEPROMFlag();
}

int getDryReading()
{
  int value = EEPROM.read(dryReadingAddress);

  if (value == 0
      || value == 255)
    return dryReading;
  else
  {
    int dryReading = value * 4; // Must multiply by 4 to get the original value
  
    if (isDebug)
    {
      Serial.print("Dry reading found in EEPROM: ");
      Serial.println(dryReading);
    }

    return dryReading;
  }
}

int getWetReading()
{
  int value = EEPROM.read(wetReadingAddress);

  int wetReading = value * 4; // Must multiply by 4 to get the original value

  if (isDebug)
  {
    Serial.print("Wet reading found in EEPROM: ");
    Serial.println(wetReading);
  }

  return wetReading;
}

void setEEPROMFlag()
{
  if (EEPROM.read(flagAddress) != 99)
    EEPROM.write(flagAddress, 99);
}

