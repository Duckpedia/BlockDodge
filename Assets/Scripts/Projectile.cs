using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.up * speed;
        }
    }

    void Update()
    {
        if (transform.position.y > 5f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Enemy>().RegisterHit();
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("CatBoss"))
        {
            collision.gameObject.GetComponent<BossManager>().RegisterHit();
            Destroy(gameObject);
        }
    }
}
