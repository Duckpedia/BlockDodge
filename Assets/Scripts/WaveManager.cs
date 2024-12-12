using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;
using System;
using Random = UnityEngine.Random;

[System.Serializable]
public struct WaveEntity {
    public GameObject type;
    public int number;
}

[System.Serializable]
public class Wave
{
    public string name;

    [Tooltip("Time till the next wave starts. Enemies spawn evenly out.")]
    public float duration;
    public WaveEntity[] entities;

    public int TotalEntities()
    {
        int n = 0;
        for (int i = 0; i < entities.Length; i++)
        {
            n += entities[i].number;
        }
        return n;
    }
}

public class WaveManager : MonoBehaviour
{
    public Wave[] waves;
    public Transform spawnPoint;
    public float spawnWidth = 16.0f;

    private int[] runtimeWaveEntityNumbers; // used during runtime to count down remaining enemy numbers
    private float nextSpawnTime;
    private float nextWaveTime;
    private int currentWaveNumber = -1;

    public static List<GameObject> activeEnemies = new List<GameObject>();

    private void Start()
    {
        currentWaveNumber = -1;
    }

    private void Update()
    {
        GameManager.Instance.enemyOnScreen = activeEnemies.Count > 0;    
        Debug.Log(activeEnemies.Count);
        if (!GameManager.gameStarted)
        {
            return;
        }

        UpdateWaves();
    }

    void UpdateWaves()
    {
        if (currentWaveNumber < 0 || currentWaveNumber >= waves.Length)
        {
            return;
        }

        if (Time.time > nextWaveTime)
        {
            if (currentWaveNumber + 1>= waves.Length) 
            {
                currentWaveNumber = -1; // done
                return;
            }

            StartWave(currentWaveNumber + 1);
        }

        if (Time.time > nextSpawnTime)
        {
            int nRemainingEnemies = NumberRemainingEntities();
            if (nRemainingEnemies == 0) return;

            int spawnedEnemy = Random.Range(0, nRemainingEnemies);
            Debug.Log("Picked:" + spawnedEnemy + "to spawn");

            for (int i = 0; i < runtimeWaveEntityNumbers.Length; i++)
            {
                int n = runtimeWaveEntityNumbers[i];
                if (n == 0) continue;

                spawnedEnemy -= n;
                if (spawnedEnemy > 0) continue;

                SpawnEnemy(waves[currentWaveNumber].entities[i].type);
                runtimeWaveEntityNumbers[i] -= 1;
                break;
            }
        }
    }

    void SpawnEnemy(GameObject go)
    {
        Debug.Log("Spawned:" + go);
        Wave wave = waves[currentWaveNumber];

        int nEntities = wave.TotalEntities();
        float spacing = wave.duration;
        if (nEntities > 0) 
        {
            spacing /= nEntities + 2;// + 2 so enemies dont spawn right at the start of the wave or right at the end
        }

        nextSpawnTime = Time.time + spacing;

        if (go == null)
        {
            return;
        }

        Vector3 spawnPos = spawnPoint.position;
        spawnPos.x = Random.Range(-spawnWidth * 0.5f, spawnWidth * 0.5f);

        GameObject newEnemy = Instantiate(go, spawnPos, Quaternion.identity);
        newEnemy.SetActive(true);

        activeEnemies.Add(newEnemy);
    }

    public void StartWave(int waveNumber)
    {
        currentWaveNumber = waveNumber;
        Debug.Log("Wave:" + waveNumber + "started");

        Wave wave = waves[currentWaveNumber];

        nextWaveTime = Time.time + wave.duration;

        runtimeWaveEntityNumbers = new int[wave.entities.Length];
        for (int i = 0; i < wave.entities.Length; i++)
        {
            runtimeWaveEntityNumbers[i] = wave.entities[i].number;
        }

        SpawnEnemy(null); // spawns a "fake" entity at the start instead of a real one
    }

    int NumberRemainingEntities()
    {
        int nRemainingEnemies = 0;
        for (int i = 0; i < runtimeWaveEntityNumbers.Length; i++)
        {
            nRemainingEnemies += runtimeWaveEntityNumbers[i];
        }
        return nRemainingEnemies;
    }

    public static void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }
}
