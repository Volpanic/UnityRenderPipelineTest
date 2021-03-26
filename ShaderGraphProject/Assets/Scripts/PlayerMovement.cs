using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 100;
    public float JumpForce = 500;
    public GameObject Capsule;
    public SquashAnStretch Squash;
    private CustomCharecterController charecterController;
    private Rigidbody body;

    public float TurnSpeed = 5;

    private Vector3 rotvec = Vector3.zero;
    private bool capsuleBeRotating = false;
    private bool landed = true;
    private float capsuleRotateTimer = 0;
    private float gBuf = 1;

    private float acceleration = 0;

    private float coyoteTimer = 0;
    private const float COYOTE_THRESH = 0.1f;

    private float jumpBufferTimer = 0;
    private float JUMP_BUFFER_TIMER = 0.1f;

    private Vector2 oldMovement = Vector2.zero;

    public Animator Animation;

    private void Awake()
    {
        charecterController = GetComponent<CustomCharecterController>();
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        Vector2 mement = new Vector2(hori, vert);

        oldMovement = Vector2.MoveTowards(oldMovement, mement, MoveSpeed *  Time.deltaTime);

        Animation.SetFloat("Xpos", oldMovement.x);
        Animation.SetFloat("Ypos", oldMovement.y);

        

        // The Aiming Stuff
        if (Input.GetMouseButtonDown(1))
        {
            Animation.SetBool("IsAiming", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Animation.SetBool("IsAiming", false);
        }


        Vector3 movement = new Vector3(0, 0, 0);

        if(vert != 0 || hori != 0)
        {
            movement += (transform.forward * vert);
            movement += (transform.right * hori);
            
            if(charecterController.Grounded)
            {
                acceleration = Mathf.MoveTowards(acceleration, 1, Time.deltaTime * 8f);
            }
            else
            {
                if(acceleration >= 1)
                {
                    acceleration += Time.deltaTime * 0.5f;
                }
                else
                {
                    acceleration = Mathf.MoveTowards(acceleration, 1, Time.deltaTime * 8f);
                }
                
            }


            movement = movement.normalized * (MoveSpeed * acceleration);

            
        }
        else if(charecterController.Grounded) //Not Moving grounded
        {
            acceleration = Mathf.MoveTowards(acceleration, 0, Time.deltaTime * 8f);
        }


        //
        if(capsuleBeRotating)
        {
            capsuleRotateTimer += Time.deltaTime;
            rotvec.x = Mathf.Lerp(0, 180, capsuleRotateTimer / 0.25f);

            //Capsule.transform.localEulerAngles = rotvec;

            if (capsuleRotateTimer >= 0.25f)
            {
                capsuleBeRotating = false;
            }
        }
        //


        charecterController.SimpleMove(movement);

        if (charecterController.Grounded && gBuf >= 0.2f)
        {
            coyoteTimer = 0;
            if (Input.GetButtonDown("Jump") || jumpBufferTimer < JUMP_BUFFER_TIMER)
            {
                Jump();   
            }
        }
        else // In Air
        {
            if (Input.GetButtonDown("Jump"))
            {
                if(coyoteTimer < COYOTE_THRESH)
                {
                    coyoteTimer = COYOTE_THRESH;
                    Jump();
                }
                else
                {
                    jumpBufferTimer = 0;
                }
            }

            coyoteTimer += Time.deltaTime;
            jumpBufferTimer += Time.deltaTime;

            gBuf += Time.deltaTime;

        }

        //if(!Animation.GetBool("Grounded"))
        //{
        //    if (charecterController.Grounded)
        //    {
        //        Animation.SetBool("Grounded", true);
        //    }
        //}
        //else
        //{
        //    if (!charecterController.Grounded)
        //    {
        //        Animation.SetBool("Grounded", false);
        //    }
        //}

        if(charecterController.Grounded || coyoteTimer < COYOTE_THRESH)
        {
            if (!landed)
            {
                landed = true;
                Squash.SquishAmount = new Vector3(1.5f, 0.5f, 1.5f);
            }
        }
        else
        {
            landed = false;
        }

        Animation.SetBool("Grounded", charecterController.Grounded || coyoteTimer < COYOTE_THRESH);

        Animation.SetFloat("Yvelocity", body.velocity.y);

    }

    public void Jump()
    {
        charecterController.Jump(JumpForce);
        capsuleBeRotating = true;
        capsuleRotateTimer = 0;
        gBuf = 0;
        Animation.SetBool("Grounded", false);
        jumpBufferTimer = JUMP_BUFFER_TIMER;
        coyoteTimer = COYOTE_THRESH;

        Squash.SquishAmount = new Vector3(0.75f,1.25f, 0.75f);
    }
}
