#ifndef SOILMOISTURESENSOR_H_
#define SOILMOISTURESENSOR_H_

extern int soilMoistureLevelCalibrated;
extern int soilMoistureLevelRaw;

extern long lastSoilMoistureSensorReadingTime;
extern long soilMoistureSensorReadingInterval;

extern int drySoilMoistureCalibrationValue;
extern int wetSoilMoistureCalibrationValue;

void setupSoilMoistureSensor();

void setupCalibrationValues();

void turnSoilMoistureSensorOn();

void turnSoilMoistureSensorOff();

void takeSoilMoistureSensorReading();

double getAverageSoilMoistureSensorReading();

double calculateSoilMoistureLevel(int soilMoistureSensorReading);

void setEEPROMIsCalibratedFlag();

void setDrySoilMoistureCalibrationValue(int drySoilMoistureCalibrationValue);

void setWetSoilMoistureCalibrationValue(int wetSoilMoistureCalibrationValue);
#endif
/* SOILMOISTURESENSOR_H_ */
