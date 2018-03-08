#ifndef COMMON_H_
#define COMMON_H_

extern bool isDebugMode;

void EEPROMWriteLong(int address, long value);
long EEPROMReadLong(int address);

#endif
/* SOILMOISTURESENSOR_H_ */
