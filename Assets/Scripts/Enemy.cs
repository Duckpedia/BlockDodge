using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0.3f;
    private int hitsRequired = 4;
    private int currentHits = 0;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Destroy(rb);
        }
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);

        if (transform.position.y < -4f)
        {
            Destroy(gameObject);
            FindFirstObjectByType<GameManager>().ResetGame();

        }
    }

    public void RegisterHit()
    {
        currentHits++;

        if (currentHits >= hitsRequired)
        {
            Destroy(gameObject);
            FindFirstObjectByType<GameManager>().EnemyDefeated();
        }
    }
}
