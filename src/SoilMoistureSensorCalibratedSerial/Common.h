#ifndef COMMON_H_
#define COMMON_H_

extern const int ANALOG_MAX;

extern long lastSerialOutputTime; // Milliseconds
extern long serialOutputInterval; // Seconds

extern bool isDebugMode;

void EEPROMWriteLong(int address, long value);
long EEPROMReadLong(int address);

long secondsToMilliseconds(int seconds);

void forceSerialOutput();

#endif
/* SOILMOISTURESENSOR_H_ */
