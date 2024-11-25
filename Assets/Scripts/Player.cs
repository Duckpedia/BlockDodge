using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private float moveSpeed = 5f;
    public float crouchSpeed = 2f;
    public float jumpForce = 1f;

    public Sprite[] walkingSprites;
    public Sprite crouchingSprite;
    public float animationSpeed = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    private bool isMoving = false;
    private Coroutine walkingCoroutine;

    public bool isGrounded = true;
    public bool canJump = true;
    private float groundCheckY = -3f;

    [SerializeField] private InputActionReference moveActionToUse;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector2 moveDirection = moveActionToUse.action.ReadValue<Vector2>();

        isGrounded = transform.position.y <= groundCheckY;

        HandleMovement(moveDirection);
        HandleJump(moveDirection);
        HandleCrouch(moveDirection);
    }

    private void HandleMovement(Vector2 moveDirection)
    {
        float horizontal = moveDirection.x;

        if (moveDirection == Vector2.zero)
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            {
                isMoving = false;
                if (walkingCoroutine != null)
                    StopCoroutine(walkingCoroutine);

                currentSpriteIndex = 0;
                spriteRenderer.sprite = walkingSprites[currentSpriteIndex];
            }
            return;
        }

        if (horizontal != 0)
        {
            rb.linearVelocity = new Vector2(horizontal * moveSpeed, rb.linearVelocity.y);

            if (!isMoving)
            {
                isMoving = true;
                walkingCoroutine = StartCoroutine(WalkingAnimation());
            }

            spriteRenderer.flipX = horizontal < 0;
        }
    }

    private void HandleJump(Vector2 moveDirection)
    {   
        if (moveDirection.y > 0.5f && isGrounded && canJump)
        {
            Vector2 jumpVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            rb.linearVelocity = jumpVelocity;
            canJump = false;
            StartCoroutine(JumpCooldown());
        }
    }

    private IEnumerator JumpCooldown()
    {
        yield return new WaitForSeconds(2f);
        canJump = true;
    }


    private void HandleCrouch(Vector2 moveDirection)
    {
        if (moveDirection.y < -0.5f)
        {
            if (walkingCoroutine != null)
                StopCoroutine(walkingCoroutine);

            isMoving = false;
            moveSpeed = crouchSpeed;
            spriteRenderer.sprite = crouchingSprite;

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size = new Vector2(collider.size.x, 0.6f);
        }
        else
        {
            moveSpeed = 5f;

            BoxCollider2D collider = GetComponent<BoxCollider2D>();
            collider.size = new Vector2(collider.size.x, 1f);

            if (Mathf.Abs(moveDirection.x) > 0.1f)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    walkingCoroutine = StartCoroutine(WalkingAnimation());
                }
            }
        }
    }

    private void ClampVerticalPosition()
    {
        float clampedY = Mathf.Clamp(transform.position.y, -3.71f, 5f);
        transform.position = new Vector3(transform.position.x, clampedY, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Block2"))
        {
            FindFirstObjectByType<GameManager>().ResetLevel();
        }
    }

    private IEnumerator WalkingAnimation()
    {
        while (true)
        {
            spriteRenderer.sprite = walkingSprites[currentSpriteIndex];
            currentSpriteIndex = (currentSpriteIndex + 1) % walkingSprites.Length;
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
