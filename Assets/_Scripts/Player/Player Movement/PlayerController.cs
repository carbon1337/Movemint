
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Scriptable Object that holds all public player variables.
    public PlayerData Data;

    //Unity Components
    [HideInInspector]
    public Rigidbody2D rb;
    private Transform groundCheck;
    private Transform wallCheck;
    public LayerMask groundLayer;

    //Float to get horizontal player input
    [HideInInspector]
    public float horizontal;

    //Variables for jumping
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    private bool performedJump;
    private bool releasedJump;

    //Variables for sliding
    private bool performedSlide;
    private bool releasedSlide = false;
    public bool canSlide = true;
    private Vector2 slideForce;
    private float forceDamping = 1.3f;

    //Bool that prevents/allows movement
    public bool canMove = true;

    //Bool for flipping character
    private bool isFacingRight = true;
    
    void Awake() {
        rb = gameObject.GetComponent<Rigidbody2D>();
        groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();
        wallCheck = GameObject.Find("WallCheck").GetComponent<Transform>();
    }

    void Update() {
        if(!isFacingRight && horizontal > 0f) {
            Flip();
        }
        else if(isFacingRight && horizontal < 0f) {
            Flip();
        }
    }

    private void FixedUpdate() {
        //If you can't move, update the x velocity to stay at 0.
        if(!canMove) {
            rb.velocity = new Vector2(0, rb.velocity.y);

            return;
        }
        
        if(!IsGrounded()) {
            canSlide = true;
        }

        HandleSlide();
		HandleMovement();
        HandleJump();
    }

    #region Jump

    public void Jump(InputAction.CallbackContext context) {

        if(context.performed) {
            performedJump = true;
        }

        if(context.canceled && rb.velocity.y > 0f) {
            releasedJump = true;
        }

    }

    private void HandleJump() 
    {
        //Reset jump variables when grounded. If not Grounded, start the coyote timer.
        if(IsGrounded()) 
        {
            coyoteTimeCounter = coyoteTime;

            releasedJump = false;
        } else 
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        //If you press the jump button, start the jump buffer counter, and reset the bools to detect if you have jumped.
        //If you aren't pressing jump, start the jump buffer timer.
        if(performedJump) 
        {
            jumpBufferCounter = jumpBufferTime;

            performedJump = false;
            releasedJump = false;
        } else 
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        //If the Jump buffer and coyote timer haven't reached zero, perform a jump.
        if(jumpBufferCounter > 0 && coyoteTimeCounter > 0) 
        {
            rb.velocity = new Vector2(rb.velocity.x, Data.jumpingPower);

            jumpBufferCounter = 0;
        }

        //If you are on the wall and you have performed a jump, wall jump.
        if(jumpBufferCounter > 0 && IsOnWall()) 
        {
            float wallJumpDirection = isFacingRight ? -1f : 1f;
            rb.velocity = new Vector2(Data.wallJumpingPower * wallJumpDirection, Data.jumpingPower);
        }

        //If you released a jump before you hit your peak, multiply the yVelocity to descend.
        if(releasedJump && rb.velocity.y > 0f) 
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);

            coyoteTimeCounter = 0f;
        }


        //If you are touching the wall but not grounded, reduce your fall speed. (Slide down the wall)
        if(IsOnWall() && rb.velocity.y != 0) 
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.8f);
        }

    }

    #endregion

    public void Move(InputAction.CallbackContext context) {
        horizontal = context.ReadValue<Vector2>().x;
    }

    private void HandleMovement() {
        float targetSpeed = horizontal * Data.runMaxSpeed;

        #region Calculate AccelRate
		float accelRate;

		//Gets an acceleration value based on if we are accelerating (includes turning) 
		//or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
		if (IsGrounded()) {
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
        } else 
        {
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;
        }
		#endregion

        #region Conserve Momentum
		//We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
		if(Data.doConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && !IsGrounded())
		{
			//Prevent any deceleration from happening, or in other words conserve are current momentum
			//You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
			accelRate = 0f; 
		}
		#endregion

		//Calculate difference between current velocity and desired velocity
		float speedDif = targetSpeed - rb.velocity.x;
		//Calculate force along x-axis to apply to thr player

		float movement = speedDif * accelRate;

		rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

    }

    public void Slide(InputAction.CallbackContext context) {
        if(context.performed && canSlide) {
            performedSlide = true;
            releasedSlide = false;
        }

        if(context.canceled) {
            releasedSlide = true;
        }
    }

    private void HandleSlide() 
    {
        //If you aren't grounded, don't run this function.
        if(!IsGrounded()) {
            return;
        }

        //When you perform a slide, add the correct force to slideforce according to the player direction.
        if(performedSlide) 
        {
            canSlide = false;
            if(isFacingRight) 
            {
                slideForce += new Vector2(Data.slidingPower, 0f);
            } else 
            {
                slideForce += new Vector2(Data.slidingPower * -1, 0f);
            }

            StartCoroutine("StartSlideCooldown");
        }

        //Add the slide force to the player
        rb.AddForce(slideForce, ForceMode2D.Force);

        //When you release the slide, slow down the force of the slide.
        if(releasedSlide) {
            slideForce /= forceDamping;
        } 
    }

    private IEnumerator StartSlideCooldown() 
    {
        performedSlide = false;
        yield return new WaitForSeconds(Data.slideCooldown);
        slideForce = Vector2.zero;
        canSlide = true;
    }


    private void Flip() 
    {
        isFacingRight=!isFacingRight;
        Vector3 localScale = transform.localScale;
        localScale.x *= -1f;
        transform.localScale = localScale;
    }

    private bool IsGrounded() 
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public bool IsOnWall() 
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.4f, groundLayer);
    }

}