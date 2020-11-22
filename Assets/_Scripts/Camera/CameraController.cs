using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables
    //Camera Movement
    [SerializeField]
    private float movementSpeed = 25;

    //Camera Roatation
    [SerializeField]
    private float mouseSensitivity = 100;
    private float camerX = 0.0f;
    private float camerY = 0.0f;

    //Speed multiplier
    [SerializeField]
    private float maxMultiplier = 2.0f;
    [SerializeField]
    private float minMultiplier = 1.0f;
    private float multiplier = 1.0f;
    #endregion

    void Start()
    {
        multiplier = minMultiplier;
        initCamPos();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        MoveCameraPos();
        RotateCamera();
        SpeedMultiplier();
    }

    void MoveCameraPos()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if((transform.position + transform.forward * Time.deltaTime * movementSpeed * multiplier).y > 2 && (transform.position + transform.forward * Time.deltaTime * movementSpeed * multiplier).y < 15)
            transform.position += transform.forward * Time.deltaTime * movementSpeed * multiplier;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            if((transform.position - transform.forward * Time.deltaTime * movementSpeed * multiplier).y > 2 && (transform.position - transform.forward * Time.deltaTime * movementSpeed * multiplier).y < 15)
            transform.position -= transform.forward * Time.deltaTime * movementSpeed * multiplier;
        }

        if (Input.GetKey(KeyCode.A))
        {
            if((transform.position - transform.right * Time.deltaTime * movementSpeed * multiplier).y > 2 && (transform.position - transform.right * Time.deltaTime * movementSpeed * multiplier).y < 15)
            transform.position -= transform.right * Time.deltaTime * movementSpeed * multiplier;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if((transform.position + transform.right * Time.deltaTime * movementSpeed * multiplier).y > 2 && (transform.position + transform.right * Time.deltaTime * movementSpeed * multiplier).y < 15)
            transform.position += transform.right * Time.deltaTime * movementSpeed * multiplier;
        }
    }

    void initCamPos()
    {
        camerX += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
        camerY -= Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

        transform.eulerAngles = new Vector3(camerY, camerX, 0.0f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void RotateCamera()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            camerX += Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensitivity;
            camerY -= Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensitivity;

            transform.eulerAngles = new Vector3(camerY, camerX, 0.0f);
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void SpeedMultiplier()
    {
        if (Input.GetKey(KeyCode.LeftShift) && multiplier != maxMultiplier)
        {
            multiplier = maxMultiplier;
        }
        else
        {
            multiplier = minMultiplier;
        }
    }

}
