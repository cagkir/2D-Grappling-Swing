using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerCam : MonoBehaviour
{

    public float sensX;
    public float sensY;

    public Transform orientation;
    public Transform Capsule;

    float xRotation;
    float yRotation;

    public Vector2 lookInput;

    public GameObject guncamrot;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);


        lookInput.x = xRotation;
        lookInput.y = yRotation;

        //guncamrot.transform.localRotation = Quaternion.Euler(xRotation,yRotation,0);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        Capsule.rotation = Quaternion.Euler(0, yRotation, 0);
    }

    public void DoFOV(float endValue)
    {
        GetComponent<Camera>().DOFieldOfView(endValue, 0.25f);
        
    }

}
