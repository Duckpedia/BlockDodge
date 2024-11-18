using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BossManager : MonoBehaviour
{
    public Transform bossTransform;
    public GameObject itemPrefab;
    public float spawnInterval = 1.5f;
    public Slider hpBar;
    public int bossHealth = 20;
    private bool isSpawningItems = false;
    private Vector2 initialPosition;
    private Vector2 targetPosition;
    private GameManager gameManager;

    void Start()
    {
        initialPosition = new Vector2(bossTransform.position.x, 10);
        targetPosition = new Vector2(bossTransform.position.x, 2.5f);
        bossTransform.position = initialPosition;
        gameManager = GameManager.Instance;

        hpBar.maxValue = bossHealth;
        hpBar.value = bossHealth;
        hpBar.gameObject.SetActive(true);
    }

    public IEnumerator BossEntrance()
    {
        float time = 0f;
        float duration = 5f;

        while (time < duration)
        {
            bossTransform.position = Vector2.Lerp(initialPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        bossTransform.position = targetPosition;
        gameManager.OnBossEntranceComplete();
        StartSpawningItems();
        StartCoroutine(MoveLeftRight());
    }

    private void StartSpawningItems()
    {
        if (!isSpawningItems)
        {
            isSpawningItems = true;
            StartCoroutine(SpawnItems());
        }
    }

    private IEnumerator SpawnItems()
    {
        while (isSpawningItems)
        {
            float bossWidth = bossTransform.localScale.x;
            Vector2 spawnPosition = new Vector2(
                bossTransform.position.x + Random.Range(-bossWidth / 2, bossWidth / 2),
                bossTransform.position.y - 0.5f);

            Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private IEnumerator MoveLeftRight()
    {

        float leftLimit = -8f;
        float rightLimit = 8f;

        float targetX = Random.Range(leftLimit, rightLimit);
        float startX = bossTransform.position.x;
        float elapsed = 0f;
        float moveDuration = 2f;

        while (true)
        {
            while (elapsed < moveDuration)
            {
                bossTransform.position = new Vector2(
                    Mathf.Lerp(startX, targetX, elapsed / moveDuration),
                    bossTransform.position.y);
                elapsed += Time.deltaTime;
                yield return null;
            }

            targetX = Random.Range(leftLimit, rightLimit);
            startX = bossTransform.position.x;
            elapsed = 0f;

            yield return new WaitForSeconds(1f);
        }
    }

    public void RegisterHit()
    {
        bossHealth--;
        hpBar.value = bossHealth;

        if (bossHealth <= 0)
        {
            StopSpawningItems();
            StopAllCoroutines();
            FindFirstObjectByType<GameManager>().OnBossDefeated();
            hpBar.gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void StopSpawningItems()
    {
        isSpawningItems = false;
    }
}
