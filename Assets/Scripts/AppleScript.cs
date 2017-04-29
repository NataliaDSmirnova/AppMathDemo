using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleScript : MonoBehaviour {

    // public variables
    public float rotationSpeed = 6.0f;
    public float maxHeight = 4.0f;
    public float step = 0.1f;
    public Vector3 movement = new Vector3(0.0f, 0.1f, 0.0f);

    // private variables
    private float currentHeight = 0.0f;

    public void Rotate()
    {
        transform.RotateAround(Vector3.zero, Vector3.up, 360.0f / rotationSpeed * Time.deltaTime);
    }

    public void Jump()
    {
        transform.Translate(movement);
        currentHeight += step;
        if (currentHeight >= maxHeight)
        {
            movement *= -1;
            currentHeight = 0.0f;
        }
    }
}
