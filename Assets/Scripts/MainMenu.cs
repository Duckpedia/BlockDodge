using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;



public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI Highscore;
    
    public void Start(){
        Highscore.text = StaticValues.HighScore.ToString();
    }

    public void PlayGame(){
        SceneManager.LoadSceneAsync("Beggining");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
