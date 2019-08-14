using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;


public class ArduinoTester : MonoBehaviour {

    int x = 0;
    int y = 0;
    string toArduino = "";

    public static SerialPort serialPort = new SerialPort("COM5", 115200, Parity.None, 8, StopBits.One);
    public static string strIn;

    // Use this for initialization
    void Start() {
        OpenConnection();
    }

    // Update is called once per frame
    void Update() {
        //serialPort.Write();
        x = Mathf.RoundToInt(Input.mousePosition.x);
        y = Mathf.RoundToInt(Input.mousePosition.y);
        if (y > 99) {
            y = 99;
        }


        toArduino = "<s," + x + "," + y + ">";
        Debug.Log(toArduino);
        serialPort.Write(toArduino);
        //Debug.Log("X: "+x+","+"Y: "+y);


    }

    public void OpenConnection() {
        if (serialPort != null) {
            if (serialPort.IsOpen) {
                serialPort.Close();
                Debug.Log("Closing port, because it was already open!");
            } else {
                serialPort.Open();
                serialPort.ReadTimeout = 50;
                Debug.Log("Port Opened!");
            }
        } else {
            if (serialPort.IsOpen) {
                print("Port is already open");
            } else {
                print("Port == null");
            }
        }
    }

    void OnApplicationQuit() {
        serialPort.Close();
        Debug.Log("Port closed!");
    }
}
