using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static bool gameStarted = false;
    public GameObject TapText;
    public TextMeshProUGUI ScoreText;

    public GameObject Block;
    public GameObject Block2;
    public GameObject Enemy;
    public GameObject ProjectilePrefab;
    public float maxX;
    public Transform SpawnPoint;
    public Transform PlayerTransform;
    public AchievementsManager achievementsManager;
    private bool enemyOnScreen = false;
    private Coroutine projectileCoroutine;
    public float spawnRate = 2f;
    public float blockSpeed = 0.5f;
    public float enemySpeed = 0.3f;
    public bool difup = false;
    public bool bossDestroyed = false;

    private int score = 0;
    public BossManager bossManager;
    public bool canShoot = false;
    public Enemy enemy;
    private bool bossActive = false;

    public void StartBossSequence()
    {
        bossActive = true;
        bossManager.gameObject.SetActive(true);
        canShoot = false;
        ScoreText.gameObject.SetActive(false);
        CancelInvoke("SpawnObject");
        StartCoroutine(bossManager.BossEntrance());
    }

    public void OnBossEntranceComplete()
    {
        print("BOSS!");
        canShoot = true;
        enemyOnScreen = true;
        if (projectileCoroutine != null)
        {
            StopCoroutine(projectileCoroutine);
        }
        projectileCoroutine = StartCoroutine(SpawnProjectiles());
    }


        public void OnBossDefeated()
    {
        canShoot = false;
        bossManager.gameObject.SetActive(false);
        ScoreText.gameObject.SetActive(true);
        enemyOnScreen = false;
        StartSpawning();
        if (projectileCoroutine != null)
        {
            StopCoroutine(projectileCoroutine);
        }
        bossDestroyed = true;
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !gameStarted)
        {
            gameStarted = true;
            StartSpawning();
            TapText.SetActive(false);
        }

        if (score >= 10 && bossDestroyed == false && !bossActive)
        {
            print("BOSS INCOMING!!");
            if(!enemyOnScreen) StartBossSequence();
        }
    }

    private void StartSpawning()
    {
        InvokeRepeating("SpawnObject", 0.4f, spawnRate);
    }

    private void SpawnObject()
    {
        if (score % 10 == 0 && !difup)
        {
            difup = true;
            IncreaseDifficulty();
        }
        else
        {
            difup = false;
        }

        if (score > StaticValues.HighScore)
        {
            StaticValues.HighScore = score;
        }
        achievementsManager.CheckForAchievements(score, StaticValues.HighScore);

        Vector3 spawnPos = SpawnPoint.position;
        spawnPos.x = Random.Range(-maxX, maxX);

        GameObject blockToSpawn = (Random.value < 0.5f) ? Block : Block2;
        Instantiate(blockToSpawn, spawnPos, Quaternion.identity);

        score++;
        ScoreText.text = score.ToString();

        if (!enemyOnScreen && Random.value >= 0.8f)
        {
            spawnPos.x = Random.Range(-maxX, maxX);
            Instantiate(Enemy, spawnPos, Quaternion.identity);
            enemyOnScreen = true;
            canShoot = true;
            if (canShoot)
            {
                projectileCoroutine = StartCoroutine(SpawnProjectiles());
            }
        }
    }

    private void IncreaseDifficulty()
    {
        blockSpeed *= 1.4f;
        enemySpeed *= 1.4f;
        spawnRate *= 0.9f;
    }

    private IEnumerator SpawnProjectiles()
    {
        if (canShoot)
        {
            yield return new WaitForSeconds(2f);
            while (canShoot)
            {
                Vector3 projectileSpawnPos = PlayerTransform.position + Vector3.up * 1f;
                Instantiate(ProjectilePrefab, projectileSpawnPos, Quaternion.identity);
                yield return new WaitForSeconds(1.5f);
            }
        }
    }


    public void EnemyDefeated()
    {
        score += 10;
        ScoreText.text = score.ToString();
        if (score > StaticValues.HighScore)
        {
            StaticValues.HighScore = score;
        }
        achievementsManager.CheckForAchievements(score, StaticValues.HighScore);
        enemyOnScreen = false;
        canShoot = false;
        if (projectileCoroutine != null)
        {
            StopCoroutine(projectileCoroutine);
        }
                
        enemy.StopAllCoroutines();
    }

    public void BackToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }

    public void OpenAchievementsPanel()
    {
        achievementsManager.OnAchievementsPanelOpened(score, StaticValues.HighScore);
    }

    public void ResetGame()
    {
        gameStarted = false;
        score = 0;
        ScoreText.text = "0";
        TapText.SetActive(true);
        spawnRate = 2f;
        blockSpeed = 0.5f;
        enemySpeed = 0.3f;
        enemyOnScreen = false;
        canShoot = false;
        ScoreText.gameObject.SetActive(true);
        bossDestroyed = false;
        bossActive = false;

        if(bossManager){
            bossManager.gameObject.SetActive(false);
            bossManager.hpBar.gameObject.SetActive(false); 
            bossManager.StopAllCoroutines(); 
        }
        
        enemy.StopAllCoroutines();
        if (projectileCoroutine != null)
        {
            StopCoroutine(projectileCoroutine);
        }

        foreach (var obj in GameObject.FindGameObjectsWithTag("Block"))
        {
            Destroy(obj);
        }
        foreach (var obj in GameObject.FindGameObjectsWithTag("Block2"))
        {
            Destroy(obj);
        }
        foreach (var obj in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            Destroy(obj);
        }
        foreach (var obj in GameObject.FindGameObjectsWithTag("Projectile"))
        {
            Destroy(obj);
        }

        CancelInvoke("SpawnObject");
    }
}
