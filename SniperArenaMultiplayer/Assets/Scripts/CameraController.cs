using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] float mouseSensitivity;
    [SerializeField] Transform player;

    float xAxisClamp = 0;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        RotateCamera();
    }

    void RotateCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        float rotAmountX = mouseX * mouseSensitivity;
        float rotAmountY = mouseY * mouseSensitivity;

        xAxisClamp -= rotAmountY;

        Vector3 rotPlayer = player.transform.rotation.eulerAngles;

        rotPlayer.y += rotAmountX;

        if (xAxisClamp > 90)
        {
            xAxisClamp = 90;
        }
        else if (xAxisClamp < -90)
        {
            xAxisClamp = -90;
        }

        player.rotation = Quaternion.Euler(rotPlayer);
    }
}
