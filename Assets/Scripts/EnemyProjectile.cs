using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public float speed = 5f; // Speed of the projectile
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.down * speed; // Move straight downward
        }
    }

    void Update()
    {
        // Destroy the projectile if it moves out of bounds
        if (transform.position.y < -5f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<GameManager>().ResetLevel();
            Destroy(gameObject);
        }
    }
}
