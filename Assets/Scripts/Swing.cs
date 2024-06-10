using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{

    public LineRenderer lr;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappable;

    public float mexSwingDistance;
    private Vector3 swingPoint;
    private SpringJoint joint;

    public Vector3 currentGrapplePos;

    PlayerMovement pm;
    public GameObject gun;
    float r = 1;

    // Start is called before the first frame update
    void Start()
    {
        pm = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            startSwing();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            stopSwing();
        }


        

        if (!joint)
        {
            float angleX = Mathf.SmoothDampAngle(gun.transform.localEulerAngles.x, 0f, ref r, 0.04f);
            float angleY = Mathf.SmoothDampAngle(gun.transform.localEulerAngles.y, 0f, ref r, 0.04f);
            float angleZ = Mathf.SmoothDampAngle(gun.transform.localEulerAngles.z, 0f, ref r, 0.04f);

            gun.transform.localRotation = Quaternion.Euler(angleX, angleY, angleZ);        
        }
        else
        {
            gun.transform.LookAt(swingPoint);
        }
        

    }

    private void LateUpdate()
    {
        DrawRope();
    }

    void startSwing()
    {
        RaycastHit hit;
        if (Physics.Raycast(cam.position, cam.forward, out hit, mexSwingDistance, whatIsGrappable))
        {

            pm.moveSpeed = 30f;

            swingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float distanceFromPoint = Vector3.Distance(player.position, swingPoint);

            joint.maxDistance = distanceFromPoint * 0.8f;
            joint.minDistance = distanceFromPoint * 0.25f;

            joint.spring = 4.5f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            lr.positionCount = 2;
            currentGrapplePos = gunTip.position;
        }
    }

    void stopSwing()
    {
        lr.positionCount = 0;
        Destroy(joint);
        pm.moveSpeed = 17;
    }


    
    void DrawRope()
    {
        if (!joint)
        {
            return;
        }

        currentGrapplePos = Vector3.Lerp(currentGrapplePos, swingPoint, Time.deltaTime * 8f);

        lr.SetPosition(0, gunTip.position);
        lr.SetPosition(1, currentGrapplePos);
    }
}
