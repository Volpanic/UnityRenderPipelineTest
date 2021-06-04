using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controls world movement and animation of the player
// using unity's built in charecter controller.

public class PlayerMovement : MonoBehaviour
{
    //Movement variables
    public float MoveSpeed = 100;
    public float JumpForce = 500;
    public SquashAnStretch Squash;
    public float TurnSpeed = 5;

    private bool landed = true;
    private CharacterController charecterController;
    private float acceleration = 0;

    // Gives a brief window when walking of ledges to jump
    // Also used for FauxGrounded because build in ground detection
    // is kind of poor.
    private float coyoteTimer = 0.11f;
    private const float COYOTE_THRESH = 0.1f;

    // Stores if the jump button was pushed in the air
    // for a short amount of time, and then jumps when landed
    private float jumpBufferTimer = 0.11f;
    private float JUMP_BUFFER_TIMER = 0.1f;

    private float yVelocity = 0; // Used for gravity and jumping
    private Vector2 oldMovement = Vector2.zero; // Smooth movement vector for animation blending

    public Animator Animation;

    // The default charecter controller grounded is clunky
    // This returns if isGrounded was set true recently
    private bool fauxGrounded
    {
        get
        {
            return charecterController.isGrounded || coyoteTimer < COYOTE_THRESH;
        }
    }

    private void Awake()
    {
        charecterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        //Move the player on the X and Y
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        Vector2 mement = new Vector2(hori, vert);

        // Smoothly move old movement towards new movement for animation blending
        oldMovement = Vector2.MoveTowards(oldMovement, mement, MoveSpeed * Time.deltaTime);

        // Set animation varaibles
        Animation.SetFloat("Xpos", oldMovement.x);
        Animation.SetFloat("Ypos", oldMovement.y);

        Vector3 movement = new Vector3(0, 0, 0);

        if (vert != 0 || hori != 0)
        {
            // Convert axis movement to world movement
            movement += (transform.forward * vert);
            movement += (transform.right * hori);

            // Gradually move to max speed
            acceleration = Mathf.MoveTowards(acceleration, 1, Time.deltaTime * 8f);

            // get the final movememnt to move the player
            movement = movement.normalized * (MoveSpeed * acceleration);
        }
        else if (fauxGrounded) //Not Moving grounded
        {
            // Gradually halt when no movement occuring
            acceleration = Mathf.MoveTowards(acceleration, 0, Time.deltaTime * 8f);
        }

        // Reset the coyote timer for fauxGrounded detection
        if (charecterController.isGrounded)
        {
            coyoteTimer = 0;
        }

        if (fauxGrounded)
        {
            // Jump if jump pressed or if pressed with the jump buffer window
            if (Input.GetButtonDown("Jump") || jumpBufferTimer < JUMP_BUFFER_TIMER)
            {
                Jump();
            }
        }
        else // In Air
        {
            //Apply gravity
            yVelocity -= 9.81f * Time.deltaTime;

            // If we just walked off a cliff, jump if jump pressed
            // else reset the jump buffer so if we land soon we jump
            if (Input.GetButtonDown("Jump"))
            {
                if (coyoteTimer < COYOTE_THRESH)
                {
                    coyoteTimer = COYOTE_THRESH;
                    Jump();
                }
                else
                {
                    jumpBufferTimer = 0;
                }
            }

            // Increase both buffers so the don't happen constently
            coyoteTimer += Time.deltaTime;
            jumpBufferTimer += Time.deltaTime;
        }

        // If we just landed, apply some squash and stretch
        if (fauxGrounded)
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

        // Move the player with movement vectory and it's y velocity
        charecterController.Move(new Vector3(movement.x, yVelocity, movement.z) * Time.deltaTime);

        // Set animation variables
        Animation.SetBool("Grounded", fauxGrounded);
        Animation.SetFloat("Yvelocity", yVelocity);

        //Quit the game
        if(Input.GetKeyDown(KeyCode.Return))
        {
            Application.Quit();
        }

    }

    public void Jump()
    {
        //Set yvelocity to jump force
        yVelocity = JumpForce;

        //Manually set animation bool
        Animation.SetBool("Grounded", false);

        //Reset the buffers
        jumpBufferTimer = JUMP_BUFFER_TIMER;
        coyoteTimer = COYOTE_THRESH;

        //Apply some squash and stretch
        Squash.SquishAmount = new Vector3(0.75f, 1.25f, 0.75f);
    }
}
