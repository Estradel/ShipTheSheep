using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class MyCameraController : MonoBehaviour
{
    public Camera camera;
    public Transform LimitTL;
    public Transform LimitBR;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.STATE == State.Pause)
        {
            return;
        }
        // Can pan the camera with drag and drop
        if (Input.GetMouseButton(0) || Input.GetMouseButton(2) || Input.GetMouseButton(1))
        {
            Vector3 translation = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0);
            translation.z = translation.y;
            translation.y = 0;
            transform.Translate(translation);
        }
        else // If the mouse is near the screen border, pan in this direction
        {
            Vector3 translation = Vector3.zero;
            if (Input.mousePosition.x < 50)
            {
                translation.x = -1 * 2;
            }
            else if (Input.mousePosition.x > Screen.width - 50)
            {
                translation.x = 1 * 2;
            }
            if (Input.mousePosition.y < 50)
            {
                translation.z = -1;
            }
            else if (Input.mousePosition.y > Screen.height - 50)
            {
                translation.z = 1;
            }
            transform.Translate(translation / 2);
        }
        // Zoom out and in with the mouse wheel at the cursor position
        if (Input.mouseScrollDelta.y != 0)
        {
            //camera.orthographicSize += Input.mouseScrollDelta.y;
            camera.transform.position += new Vector3(0, Input.mouseScrollDelta.y, 0);
            if (camera.transform.position.y < 10)
            {
                camera.transform.position -= new Vector3(0, Input.mouseScrollDelta.y, 0);
            } else if (camera.transform.position.y > 40)
            {
                camera.transform.position -= new Vector3(0, Input.mouseScrollDelta.y, 0);
            }
            // Get mouse position in screen coordinates where 0 0 is the center of the screen
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.x -= Screen.width / 2f;
            mousePosition.y -= Screen.height / 2f;
            mousePosition.z = mousePosition.y;
            mousePosition.y = 0;
            transform.Translate(mousePosition / 1000 * Math.Sign(-Input.mouseScrollDelta.y));
        }
        // Limit the camera to the limits
        if (transform.position.x < LimitTL.position.x)
        {
            transform.position = new Vector3(LimitTL.position.x, 0, transform.position.z);
        }
        if (transform.position.x > LimitBR.position.x)
        {
            transform.position = new Vector3(LimitBR.position.x, 0, transform.position.z);
        }
        if (transform.position.z < LimitTL.position.z)
        {
            transform.position = new Vector3(transform.position.x, 0, LimitTL.position.z);
        }
        if (transform.position.z > LimitBR.position.z)
        {
            transform.position = new Vector3(transform.position.x, 0, LimitBR.position.z);
        }

    }
}
