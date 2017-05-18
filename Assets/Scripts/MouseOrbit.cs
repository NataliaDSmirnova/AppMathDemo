using System;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
///     Class <c>MouseOrbit</c> implements to operate the camera.
/// </summary>
[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbit : MonoBehaviour
{
    /// <summary>
    ///     The instance variables
    ///     <c>XSpeed</c>, <c>YSpeed</c>,
    ///     <c>YMinLimit</c>, <c>YMaxLimit</c>,
    ///     <c>DistanceMin</c>, <c>DistanceMax</c>
    ///     define the parameters of the camera.
    /// </summary>
    public readonly float XSpeed = 120f;

    public readonly float YSpeed = 120f;

    public readonly float YMinLimit = -85f;
    public readonly float YMaxLimit = 85f;

    public readonly float DistanceMin = 2f;
    public readonly float DistanceMax = 15f;

    private Transform target;
    private float distance = 10f;

    private const float DeltaPosition = 0.02f;
    private const float DeltaGetAxis = 5f;

    private float xEulerAngles;
    private float yEulerAngles;
    private float zoom;

    private int lastScreenWidth;
    private int lastScreenHeight;

    private void Start()
    {
        target = transform;
        distance = -target.position.z;

        var angles = transform.eulerAngles;
        xEulerAngles = angles.x;
        yEulerAngles = angles.y;

        lastScreenWidth = Screen.width;
        lastScreenHeight = Screen.height;
    }

    private void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            //   Debug.Log("Pressed left click.");
            if (MouseIsInScreenField() && !ScreenIsResized())
            {
                SphericalMovement();
            }
        }
        else if (Math.Abs(zoom = Input.GetAxis("Mouse ScrollWheel")) > 1E-6)
        {
            //   Debug.Log("Mouse scroll wheel click.");
            Translate();
            SphericalMovement();
        }
    }

    private void SphericalMovement()
    {
        xEulerAngles += Input.GetAxis("Mouse X") * XSpeed * distance * DeltaPosition;
        yEulerAngles -= Input.GetAxis("Mouse Y") * YSpeed * DeltaPosition;

        yEulerAngles = ClampAngle(yEulerAngles, YMinLimit, YMaxLimit);

        var rotation = Quaternion.Euler(yEulerAngles, xEulerAngles, 0);

        distance = Mathf.Clamp(
            distance - Input.GetAxis("Mouse ScrollWheel") * DeltaGetAxis,
            DistanceMin,
            DistanceMax);

        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit))
            distance -= hit.distance;

        var negDistance = new Vector3(0.0f, 0.0f, -distance);
        var position = rotation * negDistance;

        transform.rotation = rotation;
        transform.position = position;
    }

    private void Translate()
    {
        var rotation = Quaternion.Euler(yEulerAngles, xEulerAngles, 0);
        distance = Mathf.Clamp(distance - zoom * DeltaGetAxis, DistanceMin, DistanceMax);

        RaycastHit hit;
        if (Physics.Linecast(target.position, transform.position, out hit))
            distance -= hit.distance;

        var negDistance = new Vector3(0.0f, 0.0f, -distance);
        Vector3 position;
        if (zoom > 0)
            position = target.position - rotation * negDistance * DeltaPosition;
        else
            position = rotation * negDistance * DeltaPosition + target.position;
        transform.position = position;
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }

    public static bool MouseIsInScreenField()
    {
        if (Input.mousePosition.x <= 0 || Input.mousePosition.x >= Screen.width)
            return false;
        if (Input.mousePosition.y <= 0 || Input.mousePosition.y >= Screen.height)
            return false;
        return true;
    }

    public bool ScreenIsResized()
    {
        if (lastScreenWidth != Screen.width || lastScreenHeight != Screen.height)
        {
            lastScreenWidth = Screen.width;
            lastScreenHeight = Screen.height;
            return true;
        }
        return false;
    }
}