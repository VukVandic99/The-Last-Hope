using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //  postavljanje osetljivosti misa, rotaciju po x i y osi
    public float mouseSensitivity = 150f;
    float xRotation = 0f;
    float yRotation = 0f;

    public float topClamp = -90f;
    //  -90 je jer kada pomeramo kameru na gore, po y osi ide u minus
    public float bottomClamp = 90f;

    void Start()
    {
        //  ne treba nam kursor:
        Cursor.lockState = CursorLockMode.Locked;

    }

    // Update is called once per frame
    void Update()
    {
        //  pomeranje po x:
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //  rotacija po x osi(gore dole), jer kada smanjujemo x osu ide na gore i povecavamo ide na dole:
        xRotation -= mouseY;

        //  blokiranje rotacije previse na gore desava se blok na 90 stepeni:
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);

        //  rotacija po y osi:
        yRotation += mouseX;

        //  primeni rotacije na nasu transofrmaciju:
        //  (x,y,z)
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
        //  Quanterion se koristi za predstavljanje rotacija u 3d prostoru
        //  euler rotacija (roll, pitch, yaw)
    }
}
