using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System.IO.Ports;
using System;

public class ArduinoThread : MonoBehaviour {
    // Singleton pattern :))
    private static ArduinoThread _Instance;
    public static ArduinoThread Instance {
        get {

            if (_Instance == null) {
                _Instance = FindObjectOfType<ArduinoThread>();
            }

            return _Instance;
        }
    }

    private void OnApplicationQuit() {
        StopThread();
    }

    private Thread thread;
    private Queue outputQueue; // From Unity to Arduino
    private Queue inputQueue; // From Arduino to Unity

    public int baudRate = 115200;
    public string port = "\\\\.\\COM5";
    private SerialPort stream;
    private int timeout = 500;

    private bool looping = true;

	public void Start() {
        outputQueue = new Queue();
        stream = new SerialPort(port, baudRate);
        stream.ReadTimeout = timeout;
        stream.Open();
        Debug.Log("Serialport opened on: " + port);
        inputQueue = Queue.Synchronized(new Queue());

        //Creates and starts the thread
        //thread = new Thread(ThreadLoop);
        //thread.Priority = System.Threading.ThreadPriority.Highest;
        //thread.Start();
    }
    
    
    public void Update() {
        // Opens the connection on the serial port

        // Looping
        //while (isLooping()) {
            // Send to Arduino
            if (outputQueue.Count != 0) {
                string command = (string) outputQueue.Dequeue();
                WriteToArduino(command);
            }  
            // Read from Arduino
            //string result = ReadFromArduino(timeout);
        //}
        
    }
        

    /**
     * Sends data to the Arduino unit
     **/
    public void SendToArduino(string command) {
        outputQueue.Enqueue(command);
    }

    public void WriteToArduino(string message) {
        stream.WriteLine(message);
        //Debug.Log("Sent " + message + " to arduino.");
        stream.BaseStream.Flush();
    }


    public void StopThread() {
        Debug.Log("ArduinoThread: " + "Serial communication stopped!");
        looping = false;
    }

    private bool isLooping() {
        return looping;
    }
}