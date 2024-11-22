using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static bool gameStarted = false;
    public GameObject TapText;

    public GameObject Block;
    public GameObject Block2;
    public GameObject Enemyobj;
    public GameObject ProjectilePrefab;
    public float maxX;
    public Transform SpawnPoint;
    public Transform PlayerTransform;
    public AchievementsManager achievementsManager;
    private bool enemyOnScreen = false;
    private Coroutine projectileCoroutine;
    public float spawnRate = 1.5f;
    public float blockSpeed = 0.5f;
    public float enemySpeed = Enemy.speed;
    public bool difup = false;
    public bool bossDestroyed = false;
    private int enemiesKilled = 0;

    private int itemsdodged = 0;
    public BossManager bossManager;
    public bool canShoot = false;
    public Enemy enemy;
    private bool bossActive = false;

    public void StartBossSequence()
    {
        bossActive = true;
        bossManager.gameObject.SetActive(true);
        canShoot = false;
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
        enemyOnScreen = false;
        StartSpawning();
        if (projectileCoroutine != null)
        {
            StopCoroutine(projectileCoroutine);
        }
        bossDestroyed = true;
        StartCoroutine(TransitionToLevel2());
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

        if (enemiesKilled == 1 && bossDestroyed == false && !bossActive)
        {
            if(!enemyOnScreen) StartBossSequence();
        }
    }

    private void StartSpawning()
    {
        InvokeRepeating("SpawnObject", 0.4f, spawnRate);
    }

    private void SpawnObject()
    {
        if (itemsdodged % 10 == 0 && !difup && itemsdodged != 0)
        {
            difup = true;
            IncreaseDifficulty();
        }
        else
        {
            difup = false;
        }

        achievementsManager.CheckForAchievements(itemsdodged);

        Vector3 spawnPos = SpawnPoint.position;
        spawnPos.x = Random.Range(-maxX, maxX);

        GameObject blockToSpawn = (Random.value < 0.5f) ? Block : Block2;
        Instantiate(blockToSpawn, spawnPos, Quaternion.identity);

        itemsdodged++;

        if (!enemyOnScreen && Random.value >= 0.8f)
        {
            spawnPos.x = Random.Range(-maxX, maxX);
            Instantiate(Enemyobj, spawnPos, Quaternion.identity);
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
        Enemy.speed *= 1.4f;
        enemySpeed = Enemy.speed;
        spawnRate *= 0.85f;
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
        enemiesKilled += 1;
        //achievementsManager.CheckForAchievements(score, StaticValues.HighScore);
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
        achievementsManager.OnAchievementsPanelOpened(itemsdodged);
    }

    private IEnumerator TransitionToLevel2()
    {
        ScreenFader fader = FindObjectOfType<ScreenFader>();
        yield return fader.FadeOut(1f);
        SceneManager.LoadScene("Level 2");
        yield return fader.FadeIn(1f);
    }


    public void ResetLevel()
    {
        gameStarted = false;
        itemsdodged = 0;
        enemiesKilled = 0;
        TapText.SetActive(true);
        spawnRate = 2f;
        blockSpeed = 0.5f;
        Enemy.speed = 0.3f;
        enemyOnScreen = false;
        canShoot = false;
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
