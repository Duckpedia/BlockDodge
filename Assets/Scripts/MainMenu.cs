using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TextMeshProUGUI Highscore;

    public void PlayGame(){
        if(StaticValues.StoryCompleted == 0)
        SceneManager.LoadSceneAsync("Beggining");
        else SceneManager.LoadSceneAsync("LevelScreen");
    }

    public void QuitGame(){
        Application.Quit();
    }
}
