using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelScreen : MonoBehaviour
{
    public void StartLevel1()
    {
        StartCoroutine(Level1()); 
    }

    private IEnumerator Level1()
    {
        ScreenFader fader = FindObjectOfType<ScreenFader>();
        yield return fader.FadeOut(1f);
        SceneManager.LoadScene("Level 1");
        yield return fader.FadeIn(1f);
    }
}
