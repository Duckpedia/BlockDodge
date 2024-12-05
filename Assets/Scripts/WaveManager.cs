using UnityEngine;
using System.Collections.Generic;

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

    public static List<GameObject> activeEnemies = new List<GameObject>();

    private void Update()
    {
        if (GameManager.gameStarted)
        {
            currentWave = waves[currentWaveNumber];
            SpawnWave();

            if (activeEnemies.Count == 0 && !canSpawn && currentWaveNumber + 1 < waves.Length)
            {
                currentWaveNumber++;
                canSpawn = true;
                GameManager.Instance.enemyOnScreen = false;
            }
            if(activeEnemies.Count != 0)
            {
                GameManager.Instance.enemyOnScreen = true;
            }
        }
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            Vector3 spawnPos = spawnPoint.position;
            spawnPos.x = Random.Range(-maxX, maxX);

            GameObject randomEnemy = currentWave.typeOfEnemies[Random.Range(0, currentWave.typeOfEnemies.Length)];
            GameObject newEnemy = Instantiate(randomEnemy, spawnPos, Quaternion.identity);

            activeEnemies.Add(newEnemy);

            currentWave.enemyNumber--;
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            if (currentWave.enemyNumber == 0)
            {
                canSpawn = false;
            }
        }
    }


    public static void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }


}
