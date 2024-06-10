using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    private PlayerMovement pm;
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappleable;

    public float maxGrappleDistance;
    public float grappleDelayTime;

    private Vector3 grapplePoint;

    public float grapplingCd;
    private float grapplingCdTimer;

    private bool grappling;
    public LineRenderer lr;

    public float overshootY;
    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            startGrapple();
        }

        if(grapplingCdTimer > 0)
        {
            grapplingCdTimer -= Time.deltaTime;
        }
    }

    private void LateUpdate()
    {
        if (grappling)
        {
            lr.SetPosition(0, gunTip.position);
        }
    }

    private void startGrapple()
    {
        if (grapplingCdTimer > 0)
        {
            return;
        }

        grappling = true;

        

        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, maxGrappleDistance, whatIsGrappleable))
        {
            grapplePoint = hit.point;
            pm.freeze = true;
            Invoke(nameof(executeGrapple), grappleDelayTime);
        }
        else
        {
            grapplePoint = cam.position + cam.forward * maxGrappleDistance;

            Invoke(nameof(stopGrapple), grappleDelayTime);
        }

        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    private void executeGrapple()
    {
        pm.freeze = false;

        Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

        float grapplePointRelativeYPos = grapplePoint.y - lowestPoint.y;
        float highestPointOnArc = grapplePointRelativeYPos + overshootY;

        if (grapplePointRelativeYPos < 0) highestPointOnArc = overshootY;

        pm.jumpToPosition(grapplePoint, highestPointOnArc);

        Invoke(nameof(stopGrapple), 1f);
    }

    public void stopGrapple()
    {
        grappling = false;
        grapplingCdTimer = grapplingCd;
        lr.enabled = false;
        pm.freeze = false;
    }
}
