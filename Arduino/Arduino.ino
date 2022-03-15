//For Arduino Uno
//A5 SCL
//A4 SDA

#include <Wire.h>
#include <MPU6050.h> //Install this library

MPU6050 mpu;

void setup() {
  Serial.begin(9600);
  Serial.println("Initialize MPU6050");
  while (!mpu.begin(MPU6050_SCALE_2000DPS, MPU6050_RANGE_2G)) {
    Serial.println("MPU6050 sensor not detected");
    delay(500);
  }
  Serial.println("MPU6050 Connected"); //Accelerometer
  //Serial.println("Initialize SW-420");
  /*
    SW-420 setup goes here
  */
  //Serial.println("MPU6050 Connected");//Vibration sensor
  //Serial.println("Initialize SIM800L");
  /*
    SIM800L setup goes here
  */
  //Serial.println("SIM800L Connected");//GSM
}

void loop() {
  String serialOut = getPitchRoll(); //Get Pitch and Roll values
  serialOut += getVibration(); //Get Vibration value
  serialOut += getLocation();//Get location coordinates
  Serial.println(serialOut); // Final Serial Like: pitch roll vibration latitude longitude altitude (1 0 3.8432 27.2046 77.4977) Split values from single space " "
}

String getPitchRoll() {
  //Accelerometer configuration gose here
  String serialOut = /*+pitch+*/" "/*+roll+*/" "; //Ends with single space " "
  return serialOut;
}

String getVibration() {
  //SW-420 configuration gose here
  String serialOut = /*+vibration+*/" "; //Ends with single space " "
  return serialOut;
}

String getLocation() {
  //GSM: get current location
  String serialOut = /*+latitude+*/" "/*+longitude+*/" "/*+altitude+*/" "; //Ends with single space " "
  return serialOut;
}

void callEmergency() {
  // GSM: calling emergency gose here
}
