#ifndef duinocom_H_
#define duinocom_H_

const int MAX_MSG_LENGTH = 10;


bool checkMsgReady();

byte* getMsg();

int getMsgLength();

void printMsg(byte msg[MAX_MSG_LENGTH]);

void clearMsg(byte msg[MAX_MSG_LENGTH]);

void identify();

char getCmdChar(byte msg[MAX_MSG_LENGTH], int characterPosition);

int readInt(byte msg[MAX_MSG_LENGTH], int startPosition, int digitCount);

#endif
