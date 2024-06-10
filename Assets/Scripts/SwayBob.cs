using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwayBob : MonoBehaviour
{
    public GameObject player;

    PlayerMovement pm;
    Rigidbody rb;


    public float step = 0.01f;
    public float maxStepDistance = 0.06f;
    Vector3 swayPos;


    public float rotationStep = 4f;
    public float maxRotationStep = 5f;
    Vector3 swayEulerRot;


    float smooth = 10f;
    float smoothRot = 12f;

    public PlayerCam cam;

    // Start is called before the first frame update
    void Start()
    {
        //pm = player.GetComponent<PlayerMovement>();
        //rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Sway();
        swayRotation();

        compositePosAndRot();
    }


    void Sway()
    {
        Vector3 InvertLook = cam.lookInput * -step;
        InvertLook.x = Mathf.Clamp(InvertLook.x, -maxStepDistance, maxStepDistance);
        InvertLook.y = Mathf.Clamp(InvertLook.y, -maxStepDistance, maxStepDistance);

        swayPos = InvertLook;
    }

    void swayRotation()
    {
        Vector2 invertLook = cam.lookInput * -rotationStep;
        invertLook.x = Mathf.Clamp(invertLook.x, -maxRotationStep, maxRotationStep);
        invertLook.y = Mathf.Clamp(invertLook.y, -maxRotationStep, maxRotationStep);

        swayEulerRot = new Vector3(invertLook.y, invertLook.x, invertLook.x);
    }

    void compositePosAndRot()
    {
        this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, swayPos, Time.deltaTime * smooth);
        this.transform.localRotation = Quaternion.Slerp(this.transform.localRotation, Quaternion.Euler(swayEulerRot), Time.deltaTime * smoothRot);
    }

}
