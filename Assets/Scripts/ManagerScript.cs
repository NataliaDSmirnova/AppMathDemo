using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerScript : MonoBehaviour {

    // public variables
    public float timeForRotation = 3.0f;
    public float timeForJumping = 8.0f;

    // private variables
    private Transform childTransform;
    private float timerCounter = 0.0f;

    void Start ()
    {
        childTransform = transform.FindChild("Apple");
	}
	
	void Update ()
    {
        timerCounter += Time.deltaTime;
        if (timerCounter < timeForRotation)
        {
            childTransform.GetComponent<AppleScript>().Rotate();
        }
        else if (timerCounter < timeForJumping)
        {
            childTransform.GetComponent<AppleScript>().Jump();
        }
        else
        {
            Vector3 centerPosition = new Vector3(childTransform.position.x, 0, childTransform.position.z);
            childTransform.Translate(centerPosition - childTransform.position);
            timerCounter = 0;
        }
    }
}
