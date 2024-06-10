using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public Transform orientation;
    public Transform playerCam;
    private Rigidbody rb;
    private PlayerMovement pm;

    public float dashForce;
    public float dashUpForce;
    public float dashDuration;

    public float dashCd;
    private float dashCdTimer;

    public PlayerCam pc;
    public float dashFOV;

    public GameObject wind;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            dash();
        }
        if(dashCdTimer > 0)
        {
            dashCdTimer -= Time.deltaTime;
        }
    }

    private void dash()
    {
        if(dashCdTimer > 0)
        {
            return;
        }
        else
        {
            dashCdTimer = dashCd;
        }
        pm.moveSpeed = 100;

        Vector3 forceToApply = playerCam.forward * dashForce + orientation.up * dashUpForce;

        rb.AddForce(forceToApply, ForceMode.Impulse);

        pc.DoFOV(dashFOV);

        wind.SetActive(true);

        Invoke(nameof(resetdash), dashDuration);
    }
    private void resetdash()
    {
        pm.moveSpeed = 17;

        pc.DoFOV(85);

        Invoke(nameof(windstop), 0.15f);
    }

    private void windstop()
    {
        wind.SetActive(false);
    }

}
