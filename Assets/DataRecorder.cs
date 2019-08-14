using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Leap;
using Leap.Unity.Interaction;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class DataRecorder : MonoBehaviour {

    public GameObject sliderMetal, sliderWood, sliderBrick, sliderWater, sliderSkin;
    private InteractionSlider intMetal, intWood, intBrick, intWater, intSkin;

    private static DataRecorder _Instance;
    public static DataRecorder Instance {
        get {

            if (_Instance == null) {
                _Instance = FindObjectOfType<DataRecorder>();
            }

            return _Instance;
        }
    }

    public GameObject nextButton;
    public InteractionButton intButtonNext;

    private StreamWriter writer;

    public GameObject metalBlock, woodBlock, brickBlock, waterBlock, skinBlock;

    private int experimentNumber = 1;

    private int lowestFrequency = 99999;
    private int highestFrequency = 0;

    private InteractionBehaviour _intObj;
    Stopwatch stopwatch;

	// Use this for initialization
	void Start () {

        intMetal = sliderMetal.GetComponent<InteractionSlider>();
        intWood = sliderWood.GetComponent<InteractionSlider>();
        intBrick = sliderBrick.GetComponent<InteractionSlider>();
        intWater = sliderWater.GetComponent<InteractionSlider>();
        intSkin = sliderSkin.GetComponent<InteractionSlider>();

        intButtonNext = nextButton.GetComponent<InteractionButton>();

        // Initial setup for which blocks should be shown
        metalBlock.SetActive(true);
        woodBlock.SetActive(false);
        brickBlock.SetActive(false);
        waterBlock.SetActive(false);
        skinBlock.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
        if (stopwatch == null) {
            stopwatch = new Stopwatch();
        }
        switch (experimentNumber) {
            case 1:
                if (intButtonNext.unpressedThisFrame) {
                    stopwatch.Start();
                    experimentOne();
                }
                break;

            case 2:
                if (intButtonNext.unpressedThisFrame && stopwatch.Elapsed.Seconds > 5) {
                    stopwatch.Reset();
                    stopwatch.Start();
                    experimentTwo();
                }
                break;

            case 3:
                if (intButtonNext.unpressedThisFrame && stopwatch.Elapsed.Seconds > 5) {
                    stopwatch.Reset();
                    stopwatch.Start();
                    experimentThree();
                }
                break;

            case 4:
                if (intButtonNext.unpressedThisFrame && stopwatch.Elapsed.Seconds > 5) {
                    stopwatch.Reset();
                    stopwatch.Start();
                    experimentFour();
                }
                break;

            case 5:
                if (intButtonNext.unpressedThisFrame && stopwatch.Elapsed.Seconds > 5) {
                    stopwatch.Reset();
                    stopwatch.Start();
                    experimentFive();
                }
                break;
        }
	}



    public void experimentOne() {
        nextExperiment();
        // Display metal block and hide all others
        Debug.Log("Ended experiment one!");
        metalBlock.SetActive(false);
        woodBlock.SetActive(true);

        // Write to log file;
        writeString("______________________________________________________________________");
        int timeInteracted = metalBlock.GetComponent<CollisionScript>().getTimeInteracted();
        writeString("Experiment 1: " + getValuesFromSliders()
            + " || " + "Elapsed time touching: " + timeInteracted.ToString() + " seconds. HIGHEST: " + highestFrequency + ", LOWEST: " + lowestFrequency);

        lowestFrequency = 99999;
        highestFrequency = 0;
    }

    public void experimentTwo() {
        nextExperiment();
        // Display wood block and hide all others
        Debug.Log("Ended experiment two!");
        woodBlock.SetActive(false);
        brickBlock.SetActive(true);

        // Write to log file;
        int timeInteracted = woodBlock.GetComponent<CollisionScript>().getTimeInteracted();
        writeString("Experiment 2: " + getValuesFromSliders()
            + " || " + "Elapsed time touching: " + timeInteracted.ToString() + " seconds. HIGHEST: " + highestFrequency + ", LOWEST: " + lowestFrequency);

        lowestFrequency = 99999;
        highestFrequency = 0;
    }

    public void experimentThree() {
        nextExperiment();
        // Display brick block and hide all others
        Debug.Log("Ended experiment three!");
        brickBlock.SetActive(false);
        waterBlock.SetActive(true);

        // Write to log file;
        int timeInteracted = brickBlock.GetComponent<CollisionScript>().getTimeInteracted();
        writeString("Experiment 3: " + getValuesFromSliders()
            + " || " + "Elapsed time touching: " + timeInteracted.ToString() + " seconds. HIGHEST: " + highestFrequency + ", LOWEST: " + lowestFrequency);

        lowestFrequency = 99999;
        highestFrequency = 0;
    }

    public void experimentFour() {
        nextExperiment();
        // Display water block and hide all others
        Debug.Log("Ended experiment four!");
        waterBlock.SetActive(false);
        skinBlock.SetActive(true);

        // Write to log file;
        int timeInteracted = waterBlock.GetComponent<CollisionScript>().getTimeInteracted();
        writeString("Experiment 4: " + getValuesFromSliders()
            + " || " + "Elapsed time touching: " + timeInteracted.ToString() + " seconds. HIGHEST: " + highestFrequency + ", LOWEST: " + lowestFrequency);

        lowestFrequency = 99999;
        highestFrequency = 0;
    }

    public void experimentFive() {
        nextExperiment();
        // Display skin block and hide all others
        Debug.Log("Ended experiment five!");
        skinBlock.SetActive(false);

        // Write to log file;
        int timeInteracted = skinBlock.GetComponent<CollisionScript>().getTimeInteracted();
        writeString("Experiment 5: " + getValuesFromSliders()
            + " || " + "Elapsed time touching: " + timeInteracted.ToString() + " seconds. HIGHEST: " + highestFrequency + ", LOWEST: " + lowestFrequency);
        writeString("______________________________________________________________________");

        lowestFrequency = 99999;
        highestFrequency = 0;
    }

    public void nextExperiment() {
           
        if (experimentNumber == 5) {
            // End experiment and write the log
            experimentNumber++;
        } else {
            // Reset values for next experiment
            experimentNumber++;

        }
    }

    /**
     * Gets the values from each of the sliders
     **/
    public string getValuesFromSliders() {
        string stringToReturn = "";
        stringToReturn += "Metal: " + intMetal.HorizontalSliderValue
            + ", Wood: " + intWood.HorizontalSliderValue
            + ", Brick:" + intBrick.HorizontalSliderValue
            + ", Water:" + intWater.HorizontalSliderValue
            + ", Skin:" + intSkin.HorizontalSliderValue;

        Debug.Log(stringToReturn);
        return stringToReturn;
    }

    public void writeString(string text) {
        string path = "C:/Users/Rendina/Desktop/test.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(text);
        writer.Close();
        //AssetDatabase.ImportAsset(path);
    }

    public void getBigAndLow(string message) {
        string[] strings = message.Split(',');
        int frequency = int.Parse(strings[1]);

        if (frequency > highestFrequency) {
            highestFrequency = frequency;
        }

        if (frequency < lowestFrequency) {
            lowestFrequency = frequency;
        }

    }
}
