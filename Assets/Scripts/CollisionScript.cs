using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap;
using Leap.Unity;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class CollisionScript : MonoBehaviour {

    Collider m_Collider;
    Vector3 m_center;
    CollisionHandler collisionHandler;
    ArduinoThread arduino;
    private bool R_thumbTouched, R_indexTouched, R_middleTouched, R_ringTouched, R_pinkyTouched;

    private DataRecorder dataRecorder;

    private Controller controller;
    private float handVelocity;
    private string serialToSend = "";

    private Stopwatch stopwatch;
    private float timeInteracted = 0;
    private bool hasBeenInteractedWith = false;

    private int combinationOfFingers = 0;

    //Variables to adjust how much the vibrators should vibrate (Hz)
    public string sineOrTri = "sine";

    private int temperature = 0;

    public int metalTemperature = 0;
    public int woodTemperature = 0;
    public int brickTemperature = 0;
    public int waterTemperature = 0;
    public int skinTemperature = 0;

    public float metalFrequency = 40.0f;
    public float woodFrequency = 40.0f;
    public float brickFrequency = 40.0f;
    public float waterFrequency = 40.0f;
    public float skinFrequency = 40.0f;

    public float metalAmplitude = 20.0f;
    public float woodAmplitude = 20.0f;
    public float brickAmplitude = 20.0f;
    public float waterAmplitude = 20.0f;
    public float skinAmplitude = 20.0f;

    // Use this for initialization
    void Start() {
        collisionHandler = CollisionHandler.Instance;
        arduino = ArduinoThread.Instance;
        dataRecorder = DataRecorder.Instance;
        m_Collider = GetComponent<Collider>();
        //arduino.StartThread();
        stopwatch = new Stopwatch();
    }

    // signal, freq, amplitude, fingercombo, temperature 

    // Update is called once per frame
    void Update() {
        temperature = 0;
        combinationOfFingers = 0;
        hasBeenInteractedWith = false;

        if (stopwatch == null) {
            stopwatch = new Stopwatch();
        }
        controller = new Controller();
        Frame frame = controller.Frame();
        serialToSend = "<";

        if (frame.Hands.Count > 0) {
            List<Hand> hands = frame.Hands;
            Hand firstHand = hands[0];
            handVelocity = getPalmVelocity(firstHand);

            serialToSend += sineOrTri + ",";

            switch (name) {
                case "Metal":
                    serialToSend += Mathf.Round(metalFrequency * handVelocity) + ",";
                    //serialToSend += Mathf.Round(metalAmplitude) + ",";
                    serialToSend += Mathf.Round(metalAmplitude) + ",";
                    temperature = metalTemperature;
                    break;

                case "Wood":
                    serialToSend += Mathf.Round(woodFrequency * handVelocity) + ",";
                    //serialToSend += Mathf.Round(metalAmplitude) + ",";
                    serialToSend += Mathf.Round(woodAmplitude) + ",";
                    temperature = woodTemperature;
                    break;

                case "Brick":
                    serialToSend += Mathf.Round(brickFrequency * handVelocity) + ",";
                    //serialToSend += Mathf.Round(brickAmplitude) + ",";
                    serialToSend += Mathf.Round(brickAmplitude) + ",";
                    temperature = brickTemperature;
                    break;

                case "Water":
                    serialToSend += Mathf.Round(waterFrequency * handVelocity) + ",";
                    //serialToSend += Mathf.Round(waterAmplitude) + ",";
                    serialToSend += Mathf.Round(waterAmplitude) + ",";
                    temperature = waterTemperature;
                    break;

                case "Skin":
                    serialToSend += Mathf.Round(skinFrequency * handVelocity) + ",";
                    //serialToSend += Mathf.Round(skinAmplitude) + ",";
                    serialToSend += Mathf.Round(skinAmplitude) + ",";
                    temperature = skinTemperature;
                    break;
            }
            // For recording biggest and lowest frequencies
            dataRecorder.getBigAndLow(serialToSend);

            //Debug.Log("Hand moves at: " + handVelocity.Magnitude);
        }

        // Pinky
        if (m_Collider.bounds.Contains(collisionHandler.R_pinkyPosition)) {
            hasBeenInteractedWith = true;
            //combinationOfFingers += 1;
            //Debug.Log("CollisionScript: R_pinky touches: " + name);
        }

        // Ring
        if (m_Collider.bounds.Contains(collisionHandler.R_ringPosition)) {
            hasBeenInteractedWith = true;
            //combinationOfFingers += 2;
            //Debug.Log("CollisionScript: R_ring touches: " + name);
        }

        // Middle
        if (m_Collider.bounds.Contains(collisionHandler.R_middlePosition)) {
            hasBeenInteractedWith = true;
            //combinationOfFingers += 4;
            //Debug.Log("CollisionScript: R_middle touches: " + name);
        }

        // Index
        if (m_Collider.bounds.Contains(collisionHandler.R_indexPosition)) {
            hasBeenInteractedWith = true;
            combinationOfFingers += 1;

            Debug.Log(name + " contains " + collisionHandler.R_indexPosition);
            Debug.Log("CollisionScript: R_index touches: " + name);
        }

        // Thumb
        if (m_Collider.bounds.Contains(collisionHandler.R_thumbPosition)) {
            hasBeenInteractedWith = true;
            combinationOfFingers += 2;
            //Debug.Log("CollisionScript: R_thumb touches " + name);

        }

        //serialToSend += ">";

        serialToSend += combinationOfFingers.ToString() + "," + temperature + ">";
            

        if (hasBeenInteractedWith) {
            arduino.SendToArduino(serialToSend);
            //Debug.Log(serialToSend);

            if (!stopwatch.IsRunning) {
                stopwatch.Start();
            }
        } else {
            arduino.SendToArduino("<sine,0,0,0,0>");
            if (stopwatch.IsRunning) {
                stopwatch.Stop();
            }
        }

        // Only send to arduino if a material has been interacted with
       
    }

    public float getPalmVelocity(Hand hand) {
        return (hand.PalmVelocity * Time.deltaTime).Magnitude;
    }

    public int getTimeInteracted() {
        int timeToReturn = stopwatch.Elapsed.Seconds;
        stopwatch.Reset();
        return timeToReturn;
    }

}