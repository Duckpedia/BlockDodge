using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float moveSpeed = 5f;
    public float jumpForce = 1f;

    public Sprite[] rightWalkingSprites;
    public Sprite[] leftWalkingSprites;
    public Sprite[] rightIdleSprites;
    public Sprite[] leftIdleSprites;
    public float animationSpeed = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    private bool isMoving = false;
    private Coroutine idleCoroutine;
    private bool idleCoroutineActive = false;

    private Coroutine walkingCoroutine;
    private bool walkingCoroutineActive = false;

    public bool isGrounded = true;
    public bool canJump = true;
    private float groundCheckY = -4.3f;

    private float moveDirection = 0f;
    private bool flipped = false;

    private Transform parentTransform;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        parentTransform = transform.parent;
    }

    void FixedUpdate()
    {
        isGrounded = parentTransform.position.y <= groundCheckY;

        rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

        if (Mathf.Abs(moveDirection) > 0.1f)
        {
            isMoving = true;
            StopIdleAnimation();
            StartWalkingAnimation();

        }
        else if (isMoving || !idleCoroutineActive)
        {
            isMoving = false;
            StopWalkingAnimation();
            StartIdleAnimation();
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

    public void OnMoveStop()
    {
        moveDirection = 0f;
    }

    public void HandleJump()
    {
        if (isGrounded && canJump)
        {
            Vector2 jumpVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            rb.linearVelocity = jumpVelocity;
            canJump = false;
            StartCoroutine(JumpCooldown());
        }
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(0.2f);
        canJump = true;
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Block2"))
        {
            FindFirstObjectByType<GameManager>().ResetLevel();
        }
    }
}
