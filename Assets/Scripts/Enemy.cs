using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public float speed = 0.3f;
    private int hitsRequired = 4;
    private int currentHits = 0;
    public Transform enemyTransform;
    private GameManager gameManager;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            Destroy(rb);
        }

        // Start hovering movement
        StartCoroutine(MoveLeftRight());
    }

    void Update()
    {
        // Check if enemy is out of bounds
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

    public IEnumerator MoveLeftRight()
    {
        float leftLimit = -8f;
        float rightLimit = 8f;

        float targetX = Random.Range(leftLimit, rightLimit);
        float startX = enemyTransform.position.x;
        float elapsed = 0f;
        float moveDuration = 4f;

        float verticalSpeed = 0.5f; // Speed of downward movement
        float hoverAmplitude = 1f; // Amplitude of the wave-like motion
        float hoverFrequency = 2f; // Frequency of the wave

        while (true)
        {
            while (elapsed < moveDuration)
            {
                // Horizontal movement
                float newX = Mathf.Lerp(startX, targetX, elapsed / moveDuration);

                // Vertical hovering movement (sinusoidal wave)
                float newY = enemyTransform.position.y - (verticalSpeed * Time.deltaTime) +
                             Mathf.Sin(elapsed * hoverFrequency) * hoverAmplitude * Time.deltaTime;

                enemyTransform.position = new Vector2(newX, newY);

                elapsed += Time.deltaTime;
                yield return null;
            }

            targetX = Random.Range(leftLimit, rightLimit);
            startX = enemyTransform.position.x;
            elapsed = 0f;
        }
    }
}
