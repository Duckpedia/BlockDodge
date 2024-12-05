using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 4f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (WaveManager.activeEnemies.Count > 0)
        {
            GameObject target = FindClosestEnemy();
            if (target != null)
            {
                Vector2 direction = (target.transform.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;
            }
        }
        else
        {
            rb.linearVelocity = Vector2.up * speed;
        }
    }


    void Update()
    {
        if (WaveManager.activeEnemies.Count > 0)
        {
            GameObject target = FindClosestEnemy();
            if (target != null)
            {
                Vector2 direction = (target.transform.position - transform.position).normalized;
                rb.linearVelocity = direction * speed;
                FindFirstObjectByType<GameManager>().SpawnProjectile();
            }
        }
        else
        {
            rb.linearVelocity = Vector2.up * speed;
        }

        if (transform.position.y > 5f)
        {
            Destroy(gameObject);
        }
    }

    public void Initialize(Vector3 direction)
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }


    GameObject FindClosestEnemy()
    {
        GameObject closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject enemy in WaveManager.activeEnemies)
        {
            if (enemy != null)
            {
                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
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
