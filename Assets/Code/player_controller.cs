using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SKCell;
using CodeMonkey.Utils;
using CodeMonkey;
using Unity.VisualScripting;

public class player_controller : MonoBehaviour
{
    public float horizontal = 0f;
    public float acceleration = 5f;
    private float decceleration = 100f;
    private float max_hspeed = 60f;
    private float max_hspeed_dash =160f;
    public float speed = 2f;
    public float jump_power = 16f;
    private float current_speed_right = 0f;
    private float current_speed_left = 0f;
    public float wall_jumping_power = 10f;
    public float air_speed = 10f;


    //Double Tap Dash
    private float lastPressTimeLeft = 0f;
    private float lastPressTimeRight = 0f;
    private const float DOUBLE_CLICK_TIME = .2f;

    private float dashingspeed = 0f;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 100f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;


    [SerializeField] private bool isFacingRight = true;

    [SerializeField] private float vspeed = 0;
    //triple Jump

    private float jumpmomemtum = 0f;

    public bool doublejump = false;
    public bool triplejump = false;
    private float jump_reset_timer = 0.01f;
    public float jump_reset = 0.5f;
    public bool start_counting = false;


    private float jump_reset_timer_triple = 0.01f;
    public float jump_reset_triple = 0.5f;
    public bool start_counting_triple = false;


    //Detect previous key input
    private bool input_right = false;
    private bool input_left = false;


    private float jump_counter = 0f;



    private float max_hspeed_running = 1.3f;
    private float accerlation_running = 80f;
    public float current_running_speed = 1f;

    public float wall_jump_delay = 0.5f;
    public float wall_jump_delay_countdown = 0f;

    private bool iswallsliding;
    private float wallSlidingSpeed = 4f;
    private float wallSliding_MaxHspseed= 3f;

    private bool isWallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingDuration = 0.2f;
    private float wallJumpingAcceleration = 1f;

    private Vector2 wallJumpingPower = new Vector2(8f, 25f);
    private Vector2 jump_velocity = new Vector2(0f, 0f);

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundcheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private Transform wallCheck_left;
    [SerializeField] private LayerMask wallLayer;

    //Animations
    public Animator animator;


    //Check if flipping
    private bool isflipping = false;

    private void Start()
    {

        wall_jump_delay_countdown = wall_jump_delay;
    }

    void Update()
    {
        if (isWallJumping)
        {
            return;
        }


        if(!isWallJumping)
        {
            jump_velocity.x = Mathf.MoveTowards(jump_velocity.x, 0, decceleration * Time.deltaTime);
        }

        if(isDashing)
        {
            if ((Input.GetKey(KeyCode.D)) && (!Input.GetKey(KeyCode.A)))
            {
                current_speed_right += acceleration * Time.deltaTime;

            }

            if ((Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.D)))
            {
                current_speed_left += acceleration * Time.deltaTime;

            }

        }
        
        
        if(IsGrounded())
        {
            jumpmomemtum = Mathf.MoveTowards(jumpmomemtum, 0, decceleration * Time.deltaTime);
        }
        
        
        rb.velocity  =  new Vector2( Mathf.Clamp(rb.velocity.x, -max_hspeed_dash, max_hspeed_dash), rb.velocity.y);
        current_speed_right = Mathf.Clamp(current_speed_right, 0, max_hspeed);
        current_speed_left = Mathf.Clamp(current_speed_left, 0, max_hspeed);


        current_running_speed = Mathf.Clamp(current_running_speed, 0, max_hspeed_running);


        if (!isDashing)
        {
            rb.gravityScale = 9.81f;

            dashingspeed = Mathf.MoveTowards(dashingspeed, 0, decceleration * Time.deltaTime * 2);

            if ((Input.GetKey(KeyCode.D)) && (!Input.GetKey(KeyCode.A)))
            {
                current_speed_right += acceleration * Time.deltaTime;
                rb.velocity = new Vector2(dashingspeed + current_speed_right * current_running_speed + jump_velocity.x + jumpmomemtum/4, rb.velocity.y);

            }
            else
            {
                current_speed_right = Mathf.MoveTowards(current_speed_right, 0, decceleration * Time.deltaTime);
            }

            if ((Input.GetKey(KeyCode.A)) && (!Input.GetKey(KeyCode.D)))
            {
                current_speed_left += acceleration * Time.deltaTime;
                rb.velocity = new Vector2(dashingspeed +  -current_speed_left * current_running_speed + jump_velocity.x + jumpmomemtum/4, rb.velocity.y);
            }
            else
            {
                current_speed_left = Mathf.MoveTowards(current_speed_left, 0, decceleration * Time.deltaTime);
            }
        }




        //Dashing

        if (Input.GetKeyDown(KeyCode.A))
        {
            float timeSinceLastPress = Time.time - lastPressTimeLeft;
            if (timeSinceLastPress <= DOUBLE_CLICK_TIME)
            {
                //Double pressed
                if(!iswallsliding)
                {
                    StartCoroutine(Dash());
                }

            }
            else
            {
                //Not Double


            }


            lastPressTimeLeft= Time.time;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            float timeSinceLastPress = Time.time - lastPressTimeRight;
            if (timeSinceLastPress <= DOUBLE_CLICK_TIME)
            {
                if (!iswallsliding)
                {
                    StartCoroutine(Dash());
                }
            }
            else
            {
                //Not Double


            }


            lastPressTimeRight = Time.time;
        }



        if (Input.GetKey(KeyCode.LeftShift) && ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))))
        {
            current_running_speed += accerlation_running * Time.deltaTime;
        }
        else
        {
            current_running_speed = 1f;
        }


        horizontal = rb.velocity.x;

        Wallslide();
        WallJump();
        if(!isWallJumping)
        {

        }

        animator.SetFloat("Speed", (current_speed_left + current_speed_right) / 2);

        if (!isWallJumping)
        {
            Flip();
            Jump();
        }

        vspeed = rb.velocity.y;
        animator.SetFloat("VerticalSpeed", vspeed);
        if (!IsGrounded() && !iswallsliding)
        {
            animator.SetBool("IsJumping", true);
        }
        else
        {
            animator.SetBool("IsJumping", false);
        }
        if (iswallsliding)
        {
            animator.SetBool("IsWallSlide", true);
        }
        else
        {
            animator.SetBool("IsWallSlide", false);
        }
    }


    private void Jump()
    {

        if (Input.GetButtonDown("Jump") && IsGrounded() && !iswallsliding)
        {
            rb.velocity = new Vector2(rb.velocity.x, jump_power);
            jumpmomemtum = rb.velocity.x;
            start_counting = true;
            jump_reset = jump_reset_timer;

        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f && !iswallsliding)
        {
            jumpmomemtum = rb.velocity.x;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }

        if (Input.GetButtonDown("Jump") && !IsGrounded() && !iswallsliding && !isWallJumping && !doublejump && (jump_reset <= 0))
        {
            jumpmomemtum = rb.velocity.x;
            doublejump = true;
            rb.velocity = new Vector2(rb.velocity.x, jump_power);
            start_counting_triple = true;
            jump_reset_triple = jump_reset_timer_triple;

        }
        if (Input.GetButtonDown("Jump") && !IsGrounded() && !iswallsliding && !triplejump && !isWallJumping && (jump_reset_triple <=0))
        {
            jumpmomemtum = rb.velocity.x;
            triplejump = true;
            rb.velocity = new Vector2(rb.velocity.x, jump_power);

        }


    }


    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundcheck.position, 0.3f, groundLayer);

    }

    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.6f, groundLayer);
    }

    private bool IsWalled_Left()
    {
        return Physics2D.OverlapCircle(wallCheck_left.position, 0.6f, groundLayer);
    }


    private void Wallslide()
    {
        if((IsWalled() && !IsGrounded() && !Input.GetButtonDown("Jump") && !IsWalled_Left()))
        {
            iswallsliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            //rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);

        }
        else if ((IsWalled_Left() && !IsGrounded() && !Input.GetButtonDown("Jump") && !IsWalled()))
        {
            iswallsliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
        }
        else
        {
            iswallsliding = false;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }


        if (start_counting == true)
        {
            jump_reset -= Time.deltaTime;
        }
        if (jump_reset <= 0)
        {
            start_counting = false;
        }

        if (start_counting_triple == true)
        {
            jump_reset_triple -= Time.deltaTime;
        }
        if (jump_reset_triple <= 0)
        {
            start_counting_triple = false;
        }


        if (IsGrounded())
        {
            doublejump = false;
            triplejump = false;
        }
        else if (isWallJumping)
        {
            doublejump = false;
            triplejump = false;
        }
  



    }

    private void WallJump()
    {
        if(iswallsliding)
        {
            //Debug.Log("Hello");
            //iswallsliding = false;
            wallJumpingCounter = wallJumpingTime;

            //CancelInvoke(nameof(StopWallJumping));

        }
        else
        {

            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {

            wall_jumping_power = 20f;
            isWallJumping = true;
            wallJumpingAcceleration += acceleration * Time.deltaTime;
            if (IsWalled())
            {
                Debug.Log("Walljump");
                //rb.AddForce(new Vector2(-40f,jump_power),ForceMode2D.Impulse);
                jump_velocity = new Vector2(transform.localScale.x *  -40f * wallJumpingAcceleration , jump_power);
               rb.velocity = new Vector2(jump_velocity.x, jump_velocity.y);
            }




            Invoke(nameof(StopWallJumping), wallJumpingDuration);

            wallJumpingCounter = 0f;
            /*
            if(transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }
           */

        }
        else if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y);
        }
    }


    private void StopWallJumping()
    {
        wallJumpingAcceleration = 1f;
        isWallJumping = false;
    }

    
    private void Flip()
    {
        if (((isFacingRight && horizontal < 0f) || (!isFacingRight && horizontal > 0f)) && !isflipping)
        {

            
            StartCoroutine(FlipOverTime(0.1f));
            isflipping= true;


        }

    }

    private IEnumerator FlipOverTime(float duration)
    {
        isFacingRight = !isFacingRight;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = new Vector3(-startScale.x, startScale.y, startScale.z);

        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            float normalizedTime = t / duration;
            // Optionally, add easing here
            // normalizedTime = Mathf.SmoothStep(0f, 1f, normalizedTime);
            transform.localScale = Vector3.Lerp(startScale, endScale, normalizedTime);
            yield return null;
        }

        // Ensure the localScale is set to the endScale when the loop is done
        isflipping = false;
        transform.localScale = endScale;
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower * current_running_speed , 0f);
        dashingspeed = rb.velocity.x;
        tr.emitting= true;
        Debug.Log($"IsGrounded: {IsGrounded()}");
        if (!IsGrounded())
        {
            Debug.Log("Triggering Air Dash Animation");
            animator.SetBool("IsDashingAir", true);
        }
        else
        {
            Debug.Log("Triggering Ground Dash Animation");
            animator.SetBool("IsDashing", true);
        }

        yield return new WaitForSeconds(dashingTime);
        animator.SetBool("IsDashingAir", false);
        animator.SetBool("IsDashing", false);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing= false;
        StartCoroutine(SmoothTransitionToMaxSpeed());
        yield return new WaitForSeconds(dashingCooldown);
        canDash= true;

    }

    private IEnumerator SmoothTransitionToMaxSpeed()
    {
        float timeToTransition = 0.5f; // Duration of the transition
        float elapsedTime = 0;

        while (elapsedTime < timeToTransition)
        {
            current_speed_left = Mathf.Lerp(current_speed_left, max_hspeed, (elapsedTime / timeToTransition));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        current_speed_left = max_hspeed;
    }

}
