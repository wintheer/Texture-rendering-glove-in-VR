using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour {
    // Singleton pattern :))
    private static CollisionHandler _Instance;
    public static CollisionHandler Instance {
        get {

            if (_Instance == null) {
                _Instance = FindObjectOfType<CollisionHandler>();
            }

            return _Instance;
        }
    }

    public GameObject L_thumbFinger, L_indexFinger, L_middleFinger, L_ringFinger, L_pinkyFinger;
    public GameObject R_thumbFinger, R_indexFinger, R_middleFinger, R_ringFinger, R_pinkyFinger;

    public Vector3 L_thumbPosition, L_indexPosition, L_middlePosition, L_ringPosition, L_pinkyPosition;
    public Vector3 R_thumbPosition, R_indexPosition, R_middlePosition, R_ringPosition, R_pinkyPosition;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // Updating the known position of each finger's tip (left hand)
        
        // Updating the known position of each finger's tip (right hand)
        R_thumbPosition = R_thumbFinger.transform.position;
        R_indexPosition = R_indexFinger.transform.position;
        R_middlePosition = R_middleFinger.transform.position;
        R_ringPosition = R_ringFinger.transform.position;
        R_pinkyPosition = R_pinkyFinger.transform.position;

    }
}