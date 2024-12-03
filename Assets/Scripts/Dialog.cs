using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class Dialog : MonoBehaviour
{

    public TextMeshProUGUI TextDisplay;
    public string[] sentences;
    private int index;
    public float TypingSpeed;

    public GameObject NextButton;

    void Start()
    {
        StartCoroutine(Type());
    }

    void Update()
    {
        if(TextDisplay.text == sentences[index])
        {
            NextButton.SetActive(true);
        }
    }

    IEnumerator Type()
    {
        foreach(char letter in sentences[index].ToCharArray())
        {
            TextDisplay.text += letter;
            yield return new WaitForSeconds(TypingSpeed);
        }
    }

    public void NextSentence()
    {
        NextButton.SetActive(false);
        if(index < sentences.Length-1)
        {
            index++;
            TextDisplay.text = "";
            StartCoroutine(Type());
        }
        else
        {
            StaticValues.StoryCompleted = 1;
            NextButton.SetActive(false);
            TextDisplay.text = "";
            StartCoroutine(Continue());              
        }
    }

    private IEnumerator Continue()
    {
        ScreenFader fader = FindObjectOfType<ScreenFader>();
        yield return fader.FadeOut(1f);
        SceneManager.LoadScene("LevelScreen");
        yield return fader.FadeIn(1f);
    }
}
