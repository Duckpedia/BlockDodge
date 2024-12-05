using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    private float moveSpeed = 5f;
    public float jumpForce = 1f;
    public float dashLengthMeters = 3.0f;
    public float dashDurationSeconds = 0.175f;

    public Sprite[] rightWalkingSprites;
    public Sprite[] leftWalkingSprites;
    public Sprite[] rightIdleSprites;
    public Sprite[] leftIdleSprites;
    public Sprite[] rightJumpSprites;
    public Sprite[] leftJumpSprites;
    public float animationSpeed = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    private bool isMoving = false;

    private Coroutine idleCoroutine;
    private bool idleCoroutineActive = false;
    private Coroutine walkingCoroutine;
    private bool walkingCoroutineActive = false;
    private Coroutine jumpingCoroutine;
    private bool jumpingCoroutineActive = false;

    public bool isGrounded = true;
    public bool canJump = true;
    private bool jumping = false;
    private float groundCheckY = -4.3f;

    private float moveDirection = 0f;
    private float lastMoveDirection = 0.0f;
    private bool flipped = false;

    private float dashTimer = 0.0f;

    private Transform parentTransform;
    public Transform playerTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        parentTransform = transform.parent;
    }

    void FixedUpdate()
    {
        isGrounded = playerTransform.position.y <= groundCheckY;

        rb.linearVelocityX = 0.0f;

        if (moveDirection != 0.0)
        {
            rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);
            lastMoveDirection = moveDirection;
            dashTimer = 0.0f;
        }

        if (dashTimer > 0.0f)
        {
            dashTimer -= Time.deltaTime;
            float dir = lastMoveDirection > 0.0f ? 1.0f : -1.0f;
            rb.linearVelocityX = (dashLengthMeters / dashDurationSeconds) * dir;
        }

        if (isGrounded && !jumping)
        {
            StopJumpingAnimation();

            if (Mathf.Abs(moveDirection) > 0.1f)
            {
                isMoving = true;
                StopIdleAnimation();
                StartWalkingAnimation();
            }
            else
            {
                isMoving = false;
                StopWalkingAnimation();
                StartIdleAnimation();
            }
        }
        else if(isGrounded && jumping)
        {
            StopIdleAnimation();
            StopWalkingAnimation();
            StartJumpingAnimation();
        }
    }

    public void OnMoveLeftDown()
    {
        flipped = false;
        moveDirection = -1f;
    }

    public void OnMoveRightDown()
    {
        flipped = true;
        moveDirection = 1f;
    }

    public void OnDash()
    {
        dashTimer = dashDurationSeconds;
    }

    public void OnMoveStop()
    {
        moveDirection = 0f;
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        canJump = true;
        jumping = false;
    }

    private void StartIdleAnimation()
    {
        if (idleCoroutine == null)
        {
            idleCoroutine = StartCoroutine(IdleAnimation());
        }
    }

    private void StartWalkingAnimation()
    {
        if (walkingCoroutine == null)
        {
            walkingCoroutine = StartCoroutine(WalkingAnimation());
        }
    }

    private void StartJumpingAnimation()
    {
        if (jumpingCoroutine == null)
        {
            jumpingCoroutine = StartCoroutine(JumpingAnimation());
        }
    }

    private void StopWalkingAnimation()
    {
        if (walkingCoroutine != null)
        {
            StopCoroutine(walkingCoroutine);
            walkingCoroutine = null;
        }
    }

    private void StopIdleAnimation()
    {
        if (idleCoroutine != null)
        {
            StopCoroutine(idleCoroutine);
            idleCoroutine = null;
        }
    }

    private void StopJumpingAnimation()
    {
        if (jumpingCoroutine != null)
        {
            Debug.Log("Stopping Jump Animation");
            StopCoroutine(jumpingCoroutine);
            jumpingCoroutine = null;
            jumpingCoroutineActive = false;
        }
    }


    private IEnumerator IdleAnimation()
    {
        idleCoroutineActive = true;
        while (true)
        {
            for (int repeat = 0; repeat < 3; repeat++)
            {
                for (int i = 0; i < 5; i++)
                {
                    spriteRenderer.sprite = flipped ? rightIdleSprites[i] : leftIdleSprites[i];
                    yield return new WaitForSeconds(animationSpeed);
                }
            }

            for (int i = 5; i < 9; i++)
            {
                spriteRenderer.sprite = flipped ? rightIdleSprites[i] : leftIdleSprites[i];
                yield return new WaitForSeconds(animationSpeed);
            }
        }
    }

    private IEnumerator WalkingAnimation()
    {
        walkingCoroutineActive = true;
        while (true)
        {
            for (int i = 0; i < 7; i++)
            {
                spriteRenderer.sprite = flipped ? rightWalkingSprites[i] : leftWalkingSprites[i];
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    private IEnumerator JumpingAnimation()
    {
        jumpingCoroutineActive = true;

        for (int i = 0; i < rightJumpSprites.Length; i++)
        {
            spriteRenderer.sprite = flipped ? rightJumpSprites[i] : leftJumpSprites[i];
            yield return new WaitForSeconds(0.06f);
        }
    }


    public void HandleJump()
    {
        if (isGrounded && canJump)
        {
            jumping = true;
            StartCoroutine(DelayedJump());
        }
    }

    private IEnumerator DelayedJump()
    {
        yield return new WaitForSeconds(0.2f);

        Vector2 jumpVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        rb.linearVelocity = jumpVelocity;
        canJump = false;
        StartCoroutine(JumpCooldown());
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Block2"))
        {
            FindFirstObjectByType<GameManager>().ResetLevel();
        }
    }
}
