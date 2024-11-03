using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject Block;
    public float maxX;
    public Transform SpawnPoint;
    public float spawnRate;


    bool gameStarted = false;

    public GameObject TapText;
    public TextMeshProUGUI ScoreText;

    int score = -1;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) && !gameStarted)
        {
            gameStarted = true;
            StartSpawning();
            TapText.SetActive(false);
        }
        
    }

    private void StartSpawning()
    {
        InvokeRepeating("SpawnBlock", 0.4f, spawnRate);
    }

    private void SpawnBlock()
    {
        Vector3 spawnPos = SpawnPoint.position;
        
        spawnPos.x = Random.Range(-maxX, maxX);

        Instantiate(Block, spawnPos, Quaternion.identity);

        score++;
        ScoreText.text = score.ToString();
        if(score > StaticValues.HighScore){
            StaticValues.HighScore = score;
        }
    }

    public void BackToMenu(){
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
