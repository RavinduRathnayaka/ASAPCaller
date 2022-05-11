#include <ArduinoWiFiServer.h>
#include <BearSSLHelpers.h>
#include <CertStoreBearSSL.h>
#include <ESP8266WiFi.h>
#include <ESP8266WiFiAP.h>
#include <ESP8266WiFiGeneric.h>
#include <ESP8266WiFiGratuitous.h>
#include <ESP8266WiFiMulti.h>
#include <ESP8266WiFiScan.h>
#include <ESP8266WiFiSTA.h>
#include <ESP8266WiFiType.h>
#include <WiFiClient.h>
#include <WiFiClientSecure.h>
#include <WiFiClientSecureBearSSL.h>
#include <WiFiServer.h>
#include <WiFiServerSecure.h>
#include <WiFiServerSecureBearSSL.h>
#include <WiFiUdp.h>

//For Arduino Uno
//A5 SCL
//A4 SDA

#include <Wire.h>
#include <MPU6050.h> //Install this library

MPU6050 mpu;

void setup() {
  Serial.begin(9600);
  Serial.println("Connecting to WPF");
  validatePortConnection();
  Serial.println("Connected to WPF");
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
  wpfSerialRead();
  String serialOut="Reading>> ";
  serialOut += getPitchRoll(); //Get Pitch and Roll values
  serialOut += getVibration(); //Get Vibration value
  serialOut += getLocation();//Get location coordinates
  Serial.println(serialOut); // Final Serial Like: pitch roll vibration latitude longitude altitude (1 0 3.8432 27.2046 77.4977) Split values from single space " "
}

void validatePortConnection(){
  Serial.print("Response>> ");
  while(true){
    char serialIn=Serial.read();
    if(serialIn=='C'){ //Sent by WPF
      break;
    }
  }
}

void wpfSerialRead(){
  Serial.print("Response>> ");
  while(true){
    char serialIn=Serial.read();
    if(serialIn=='c'){  //Continue Reading
      Serial.println("Continue Reading");
      break;
    }
    if(serialIn=='p'){  //Pause the device
      Serial.println("Pause Reading");
      pauseDevice();
      break;
    }
    if(serialIn=='e'){  //Call Emergency
      Serial.println("Call Emergency");
      callEmergency();
    }
  }
}

void pauseDevice(){
  Serial.print("Response>> ");
  while(true){
    char serialIn=Serial.read();
    if(serialIn=='c'){
      Serial.println("Continue Reading");
      break;
    }
  }
}

String getPitchRoll() {
// Read Raw Values
  Vector raw = mpu.readNormalizeAccel();
  // Calculate Pitch & Roll
  int pitch = -(atan2(raw.XAxis, sqrt(raw.YAxis * raw.YAxis + raw.ZAxis * raw.ZAxis)) * 180.0) / M_PI;
  int roll = (atan2(raw.YAxis, raw.ZAxis) * 180.0) / M_PI;
  // Output
  String serialOut = (String)pitch + " " + (String)roll + " "; //Split values from single space " "
  return serialOut; //Return as a string
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
  afterCallAction();
}

void afterCallAction(){
  // Instructions after calling emergency
}
