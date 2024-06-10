using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float GroundDrag;

    public float JumpForce;
    public float JumpCoolDown;
    public float airMultiplier;
    public bool readyToJump;

    [Header("Ground Check")]
    public float PlayerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;

    public bool freeze;
    public bool activeGrapple;

    private bool enablemovementNextTouch;

    public PlayerCam playerCam;
    public float grapFOV;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, PlayerHeight * 0.5f + 0.2f, whatIsGround);

        if (freeze)
        {
            rb.velocity = Vector3.zero;
        }
        else
        {
            MyInput();
            speedControl();
        }     

        if (grounded && !activeGrapple)
        {
            rb.drag = GroundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        movePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), JumpCoolDown);
        }
    }

    private void movePlayer()
    {
        if (activeGrapple)
        {
            return;
        }

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
        
    }

    private void speedControl()
    {
        if (activeGrapple)
        {
            return;
        }

        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * JumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void jumpToPosition(Vector3 TargetPos, float TrajectoryHeight)
    {
        activeGrapple = true;

        velocityToSet = CalculateJumpVelocity(transform.position, TargetPos, TrajectoryHeight);
        Invoke(nameof(SetVel), 0.1f);

        Invoke(nameof(resetRest), 3f);
    }

    public void resetRest()
    {
        activeGrapple = false;

        playerCam.DoFOV(85f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (enablemovementNextTouch)
        {
            enablemovementNextTouch = false;
            resetRest();

            GetComponent<Grappling>().stopGrapple();
        }
    }

    private Vector3 velocityToSet;
    void SetVel()
    {
        enablemovementNextTouch = true;

        rb.velocity = velocityToSet;

        playerCam.DoFOV(grapFOV);
    }

    public Vector3 CalculateJumpVelocity(Vector3 startPoint, Vector3 endPoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endPoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endPoint.x - startPoint.x, 0f, endPoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity)
            + Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityXZ + velocityY;
    }


}
