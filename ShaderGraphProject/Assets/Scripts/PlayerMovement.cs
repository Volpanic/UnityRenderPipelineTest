using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 100;
    public GameObject Capsule;
    private CustomCharecterController charecterController;

    public float TurnSpeed = 5;

    private Vector3 rotvec = Vector3.zero;
    private bool capsuleBeRotating = false;
    private float capsuleRotateTimer = 0;
    private float gBuf = 1;

    private float acceleration = 0;

    private float coyoteTimer = 0;
    private const float COYOTE_THRESH = 0.1f;

    private float jumpBufferTimer = 0;
    private float JUMP_BUFFER_TIMER = 0.1f;

    public Animator Animation;

    private void Awake()
    {
        charecterController = GetComponent<CustomCharecterController>();
    }

    void Update()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

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

            Animation.SetFloat("Xpos", Input.GetAxis("Horizontal"));
            Animation.SetFloat("Ypos", Input.GetAxis("Vertical"));
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
    }

    public void Jump()
    {
        charecterController.Jump(500);
        capsuleBeRotating = true;
        capsuleRotateTimer = 0;
        gBuf = 0;
        jumpBufferTimer = JUMP_BUFFER_TIMER;
    }
}
