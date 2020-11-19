using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    int speed = 0;
    int min_speed = -9;
    int max_speed = 9;
    int boost_speed = 2;
    int degrees = 10;

    // Use this for initialization
    void Start()
    {

    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed, Space.Self);

        if (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.UpArrow)))
        {
            transform.Translate(Vector3.forward * Time.deltaTime * (speed+boost_speed), Space.Self);
        }
        if (Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.DownArrow)))
        {
            speed = 0;
        }
        if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow)))
        {
            transform.Translate(Vector3.left * Time.deltaTime * speed, Space.Self);
        }
        if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow)))
        {
            transform.Translate(Vector3.right * Time.deltaTime * speed, Space.Self);
        }
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            speed += 1;
            if (speed > max_speed)
            {
                speed = max_speed;
            }
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            speed -= 1;
            if (speed < min_speed)
            {
                speed = min_speed;
            }
        }
        if (Input.GetMouseButton(1))  // use rmb to rotate view
        {
            if (Input.GetAxis("Mouse X") > 0)
            {
                transform.RotateAround(transform.position, transform.up, Input.GetAxis("Mouse X") * degrees);
            }
            if (Input.GetAxis("Mouse X") < 0)
            {
                transform.RotateAround(transform.position, transform.up, Input.GetAxis("Mouse X") * degrees);
            }
            if (Input.GetAxis("Mouse Y") > 0)
            {
                transform.RotateAround(transform.position, transform.right, Input.GetAxis("Mouse Y") * -degrees);
            }
            if (Input.GetAxis("Mouse Y") < 0)
            {
                transform.RotateAround(transform.position, transform.right, Input.GetAxis("Mouse Y") * -degrees);
            }
        }
    }
}