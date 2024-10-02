using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    [SerializeField] float minJumpHeight = 2;
    [SerializeField] float maxJumpHeight = 4;
    [SerializeField] float timeToJumpApex = 0.4f;
    [SerializeField] float moveSpeed = 6;
    [SerializeField] float accelerationTimeAirborne = 0.2f;
    [SerializeField] float accelerationTimeGrounded = 0.1f;

    [SerializeField] Vector2 wallJumpClimb;
    [SerializeField] Vector2 wallJumpOff;
    [SerializeField] Vector2 wallLeap;

    [SerializeField] float wallSlideSpeedMax = 3;
    [SerializeField] float wallStickTime = 0.25f;

    [SerializeField] float coyoteTime = 0.5f;

    [SerializeField] float jumpBufferTime = 0.5f;
    [SerializeField] float jumpCooldown;

    [SerializeField] float dashVelocity = 3f;
    [SerializeField] float dashTime = 0.1f;
    [SerializeField] float dashCooldown = 0.5f;

    float gravity;
    float maxJumpVelocity;
    float minJumpVelocity;
    float velocityXSmoothing;
    Vector3 velocity;
    float timeToWallUnstick;
    float coyoteTimer;
    float jumpBufferTimer;
    bool isDashing;
    bool isDashOnCooldown;

    Controller2D controller2D;
    Vector2 directionalInput;
    bool wallSliding;
    int wallDirX;

    bool isJumped;

    Animator animator;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        controller2D = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        gravity = -(2 * maxJumpHeight) / (timeToJumpApex * timeToJumpApex);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    void FixedUpdate()
    {
        CalculateVelocity();
        HandleWallSliding();
        HandleCoyote();
        HandleBuffer();

        if (coyoteTimer > 0f && jumpBufferTimer > 0f && !isJumped)
        {
            Jump();
            jumpBufferTimer = 0f;
        }

        controller2D.Move(velocity * Time.deltaTime, directionalInput);

        if (controller2D.collisions.above || controller2D.collisions.below)
        {
            if (controller2D.collisions.slidingDownMaxSlope)
                velocity.y += controller2D.collisions.slopeNormal.y * -gravity * Time.deltaTime;
            else
                velocity.y = 0;
        }
    }
    void HandleBuffer()
    {
        if (jumpBufferTimer > 0f)
            jumpBufferTimer -= Time.deltaTime;
    }
    void HandleCoyote()
    {
        if (controller2D.collisions.below)
            coyoteTimer = coyoteTime;
        else
            coyoteTimer -= Time.deltaTime;
    }

    void HandleDashing()
    {
        if (isDashing)
        {
            velocity.x = dashVelocity * controller2D.collisions.faceDir;
            velocity.y = 0;
        }
    }

    void Update()
    {
        if (directionalInput.x > 0)
            spriteRenderer.flipX = false;
        else if (velocity.x < 0)
            spriteRenderer.flipX = true;

        animator.SetFloat("VelocityX", Mathf.Abs(directionalInput.x));

        if (!controller2D.collisions.below)
        {
            animator.SetFloat("VelocityY", velocity.y);
            animator.SetBool("OnGround", false);
        }
        else
            animator.SetBool("OnGround", true);
    }

    void Jump()
    {
        velocity.y = maxJumpVelocity;
        isJumped = true;
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    void ResetJump()
    {
        isJumped = false;
    }


    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        jumpBufferTimer = jumpBufferTime;

        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }

        if (controller2D.collisions.below)
        {
            Jump();
            coyoteTimer = 0f;
        }
    }

    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
            velocity.y = minJumpVelocity;
    }

    public void OnDashInputDown()
    {
        if (isDashOnCooldown)
            return;

        isDashOnCooldown = true;
        isDashing = true;

        Invoke(nameof(ResetDashing), dashTime);
        Invoke(nameof(ResetDashCooldown), dashCooldown);
    }

    void ResetDashing()
    {
        isDashing = false;
    }
    void ResetDashCooldown()
    {
        isDashOnCooldown = false;
    }

    void HandleWallSliding()
    {
        wallDirX = controller2D.collisions.left ? -1 : 1;
        wallSliding = false;
        if ((controller2D.collisions.left || controller2D.collisions.right) && !controller2D.collisions.below && velocity.y < 0)
        {
            wallSliding = true;

            if (velocity.y < -wallSlideSpeedMax)
                velocity.y = -wallSlideSpeedMax;

            if (timeToWallUnstick > 0)
            {
                velocityXSmoothing = 0;
                velocity.x = 0;

                if (directionalInput.x != wallDirX && directionalInput.x != 0)
                {
                    timeToWallUnstick -= Time.deltaTime;
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            }
            else
            {
                timeToWallUnstick = wallStickTime;
            }
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, controller2D.collisions.below ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }
}
