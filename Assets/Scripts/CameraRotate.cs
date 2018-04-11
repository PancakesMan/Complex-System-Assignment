using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform target;
    public float speed = 10;
    public float distance = 5;
    public float distanceMin = 2;
    public float distanceMax = 20;
    public float heightOffset = 1.5f;
    public float zoomSpeed = 100;

    float lastMouseX;
    float lastMouseY;

    // Update is called once per frame
    void Update()
    {
        // zoom in and out with scroll wheel
        distance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime;
        distance = Mathf.Clamp(distance, distanceMin, distanceMax);

        // if the right button's down, rotate around the target
        if (Input.GetMouseButton(1))
        {
            // TODO - this causes weird jumps if you lose focus and regain it.
            float deltaX = Input.mousePosition.x - lastMouseX;
            float deltaY = Input.mousePosition.y - lastMouseY;

            // set the camera's rotation
            Vector3 angles = transform.eulerAngles + (Vector3.right * deltaY + Vector3.up * deltaX) * Time.deltaTime * speed;
            if (angles.x > 180)
                angles.x -= 360;
            angles.x = Mathf.Clamp(angles.x, -70, 70);
            transform.eulerAngles = angles;
        }
        // position the camera looking at the target, with a height offset so we don't focus on their feet
        transform.position = target.position + Vector3.up * heightOffset - transform.forward * distance;

        // store m,ouse position for next frame
        lastMouseX = Input.mousePosition.x;
        lastMouseY = Input.mousePosition.y;
    }

}