using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public int enemyNumber;
    public GameObject[] typeOfEnemies;
    public float spawnInterval;
}

public class WaveManager : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnPoint;

    private Wave currentWave;
    private int currentWaveNumber;
    private float nextSpawnTime;

    private float maxX = 8f;
    private bool canSpawn = true;

    private void Update()
    {
        if(GameManager.gameStarted == true){
            currentWave = waves[currentWaveNumber];
            SpawnWave();
            GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            if(totalEnemies.Length == 0 && !canSpawn && currentWaveNumber + 1 != waves.Length)
            {
                currentWaveNumber++;
                canSpawn = true;
            }
        }
    }

    void SpawnNextWave()
    {
        currentWaveNumber++;
        canSpawn = true;
    }

    void SpawnWave()
    {
        if(canSpawn && nextSpawnTime < Time.time)
        {
            Vector3 spawnPos = spawnPoint.position;
            spawnPos.x = Random.Range(-maxX, maxX);
            
            GameObject randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            Instantiate(randomEnemy, spawnPos, Quaternion.identity);
            
            currentWave.enemyNumber--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;
            
            if(currentWave.enemyNumber == 0)
            {
                canSpawn = false;
            }
        }
    }
}
