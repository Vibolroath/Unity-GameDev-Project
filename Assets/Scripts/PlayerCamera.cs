using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform lookAt;
    public Transform camTransform;

    private Camera cam;

    private float distance = 10.0f;
    private float currX = 0.0f;
    private float currY = 0.0f;

    private const float yMin = 0.0f;
    private const float yMax = 75.0f;

    void Start()
    {
        camTransform = transform;
        cam = Camera.main;

        //Pressing left mouse button makes the cursor invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        //Pressing escape makes the cursor appear again
        if (Input.GetButtonDown("Cancel"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        //Get the mouse axes
        currX += Input.GetAxis("Mouse X");
        currY += Input.GetAxis("Mouse Y");

        //Clamp the axes so the rotating camera does not loops
        currY = Mathf.Clamp(currY, yMin, yMax);
    }

    void LateUpdate()
    {
        //Statements for keeping player in the middle of the screen and rotating with mouse
        if (lookAt != null)
        {
            Vector3 dir = new Vector3(0, 0, -distance);
            Quaternion rotation = Quaternion.Euler(currY, currX, 0);
            camTransform.position = lookAt.position + rotation * dir;
            camTransform.LookAt(lookAt.position);
        }
    }
}
