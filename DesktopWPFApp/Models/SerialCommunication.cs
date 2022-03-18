using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DesktopWPFApp.Models {
    public class SerialCommunication {
        public bool AccidentDetected = false;
        public bool DriverInCar = true;
        public bool Conntected=false;
        #region Port setup
        public string[] SerialPorts { get; set; }
        private SerialPort port = new SerialPort() {
            //TEST: Hardcoded port name PortName = "COM3",  
            BaudRate = 9600,    //Same as Arduino
            DtrEnable = true
        };
        public SerialCommunication() {
            PortUpdate();    //GET: Any available serial port names
            port.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
            port.PinChanged += new SerialPinChangedEventHandler(PinChanged);
            Conntected = port.IsOpen;
        }

        private void PinChanged(object sender, SerialPinChangedEventArgs e) {
            PortUpdate();
            if(SerialPorts==null || SerialPorts.Length == 0) {
                if (port.IsOpen) {
                    PortDisConnect();
                }
                AccidentDetected = true;
            }
        }
        public void PortUpdate() {
            SerialPorts = SerialPort.GetPortNames();
        }
        public void ChangePort(string aPortName) {
            port.PortName = aPortName;
        }
        public void PortConnect() {
            try {
                port.Open();
                Conntected= port.IsOpen;
            }
            catch (Exception) {
                throw new Exception("Cannot connect to Device");    //TODO: Update Ui
            }
        }
        public void PortDisConnect() {
            try {
                port.Close();
                Conntected = port.IsOpen;
            }
            catch (Exception) {
                throw new Exception("Cannot disconnect");   //TODO: Update Ui
            }
        }
        #endregion
        #region Arduino Connection
        #region Sensor values
        public int Pitch { get; set; }
        public int Roll { get; set; }
        public float Vibration { get; set; }
        public float Latitude { get; set; }
        public float Longitude { get; set; }
        public float Altitude { get; set; }
        public bool PIR { get; set; }
        #endregion
        private void SetSensorValues(string[] aSerialIn) {
            Pitch = int.Parse(aSerialIn[1]);
            Roll = int.Parse(aSerialIn[2]);
            //TODO: Vibration=float.Parse(aSerialIn[3]);
            //TODO: Latitude = float.Parse(aSerialIn[4]);
            //TODO: Longitude = float.Parse(aSerialIn[5]);
            //TODO: Altitude = float.Parse(aSerialIn[6]);
            //TODO: PIR=bool.Parse(aSerialIn[7]);
        }
        #region Detect Accident
        private void CheckSensorValues() {
            if (Pitch >= 85 || Pitch <= -85) {
                SerialWrite("p");
                AccidentDetected = true;
            }
            else if (Roll >= 85 || Roll <= -85) {
                SerialWrite("p");
                AccidentDetected = true;
            }
            else if (Vibration >= 100) {
                SerialWrite("p");
                AccidentDetected = true;
            }
        }
        #endregion
        #region Serial Communication
        private void SerialWrite(string aSerial) {
            if (!String.IsNullOrEmpty(aSerial) || port.IsOpen) {
                port.WriteLine(aSerial);
            }
        }
        private void SerialRead() {
            string? serial = null;
            try {
                serial = port.IsOpen ? port.ReadLine() : null;
            }
            catch {
                serial = null;
            }
            
            if (!String.IsNullOrEmpty(serial)) {
                if (serial.Contains("Reading>>") && !serial.Contains("Pause")) {
                    string[] SerialIn = serial.Split(" ");
                    SetSensorValues(SerialIn);
                    CheckSensorValues();
                    if (AccidentDetected) return;
                    SerialWrite("c");
                }
                else if (serial.Contains("Connected")) {
                    SerialWrite("c");
                }
                else if (serial.Contains("Connecting to WPF")) {
                    SerialWrite("C");
                }
            }
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e) {
            if (port.IsOpen) {
                SerialRead();
            }
        }
        #endregion
        #endregion
    }
}

