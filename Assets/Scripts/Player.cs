using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float moveSpeed = 5f;
    public Sprite[] walkingSprites;
    public float animationSpeed = 0.2f;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private int currentSpriteIndex = 0;
    private bool isMoving = false;
    private Coroutine walkingCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float moveDirection = touchPos.x < 0 ? -1 : 1;
            rb.linearVelocity = new Vector2(moveDirection * moveSpeed, rb.linearVelocity.y);

            if (!isMoving)
            {
                isMoving = true;
                walkingCoroutine = StartCoroutine(WalkingAnimation());
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            if (isMoving)
            {
                isMoving = false;
                StopCoroutine(walkingCoroutine);
                currentSpriteIndex = 0;
                spriteRenderer.sprite = walkingSprites[currentSpriteIndex];
            }
        }

    }

    IEnumerator WalkingAnimation()
    {
        while (true)
        {
            spriteRenderer.sprite = walkingSprites[currentSpriteIndex];
            currentSpriteIndex = (currentSpriteIndex + 1) % walkingSprites.Length;
            yield return new WaitForSeconds(animationSpeed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Block" || collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "Block2")
        {
            FindFirstObjectByType<GameManager>().ResetGame();
        }
    }
}
