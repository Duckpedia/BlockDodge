using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public static float speed = 0.3f;
    private int hitsRequired = 4;
    private int currentHits = 0;
    public Transform enemyTransform;
    private GameManager gameManager;
    private Vector2 previousPosition;
    private float horizontalDirection;

    private Coroutine flyingCoroutine;
    private Coroutine shootingCoroutine;
    public Sprite[] leftFlyingSprites;
    public Sprite[] rightFlyingSprites;
    private SpriteRenderer spriteRenderer;
    public GameObject projectilePrefab; // Assign your projectile prefab in the Inspector
    public float shootingInterval = 1.0f;

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (rb != null)
        {
            Destroy(rb);
        }
        StartCoroutine(MoveLeftRight());
        StartFlyingAnimation();
        StartShooting();
    }

    void Update()
    {
        if (transform.position.y < -4f)
        {
            Destroy(gameObject);
            FindFirstObjectByType<GameManager>().ResetLevel();
        }
    }

    public void RegisterHit()
    {
        currentHits++;
        Debug.Log($"Enemy hit! Current hits: {currentHits}/{hitsRequired}");

        if (currentHits >= hitsRequired)
        {
            Debug.Log("Enemy destroyed!");
            Destroy(gameObject);
            FindFirstObjectByType<GameManager>().EnemyDefeated();
            WaveManager.RemoveEnemy(gameObject);
        }
    }

    public IEnumerator MoveLeftRight()
    {
        float leftLimit = -8f;
        float rightLimit = 8f;

        float targetX = Random.Range(leftLimit, rightLimit);
        float startX = transform.position.x;
        float elapsed = 0f;
        float moveDuration = 4f;

        float verticalSpeed = speed;
        float hoverAmplitude = 1f;
        float hoverFrequency = 2f;

        previousPosition = transform.position;

        while (true)
        {
            while (elapsed < moveDuration)
            {
                float newX = Mathf.Lerp(startX, targetX, elapsed / moveDuration);

                float newY = transform.position.y - (verticalSpeed * Time.deltaTime) +
                             Mathf.Sin(elapsed * hoverFrequency) * hoverAmplitude * Time.deltaTime;

                Vector2 newPosition = new Vector2(newX, newY);

                horizontalDirection = Mathf.Sign(newPosition.x - previousPosition.x);

                transform.position = newPosition; // Update position of the enemy.

                previousPosition = newPosition;

                elapsed += Time.deltaTime;
                yield return null;
            }

            targetX = Random.Range(leftLimit, rightLimit);
            startX = transform.position.x;
            elapsed = 0f;
        }
    }

    private IEnumerator FlyingAnimation()
    {
        while (true)
        {
            Sprite[] currentSprites = horizontalDirection > 0 ? rightFlyingSprites : leftFlyingSprites;
            for (int i = 0; i < currentSprites.Length; i++)
            {
                spriteRenderer.sprite = currentSprites[i];
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    private void StartFlyingAnimation()
    {
        if (flyingCoroutine == null)
        {
            flyingCoroutine = StartCoroutine(FlyingAnimation());
        }
    }

    private void StopFlyingAnimation()
    {
        if (flyingCoroutine != null)
        {
            StopCoroutine(flyingCoroutine);
            flyingCoroutine = null;
        }
    }

    private IEnumerator ShootProjectiles()
    {
        while (true)
        {
            if (projectilePrefab != null)
            {
                Vector3 spawnPosition = new Vector3(transform.position.x, transform.position.y - 0.5f, 0);
                Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);
            }
            yield return new WaitForSeconds(shootingInterval);
        }
    }

    private void StartShooting()
    {
        if (shootingCoroutine == null)
        {
            shootingCoroutine = StartCoroutine(ShootProjectiles());
        }
    }

    private void StopShooting()
    {
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    void OnDestroy()
    {
        WaveManager.RemoveEnemy(gameObject);
        if (WaveManager.activeEnemies.Count == 0)
        {
            GameManager.Instance.enemyOnScreen = false;
        }
        StopFlyingAnimation();
        StopShooting();
    }
}
    