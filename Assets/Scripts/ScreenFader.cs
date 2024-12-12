using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage; // Assign the UI Image here

    public IEnumerator FadeOut(float duration)
    {
        fadeImage.gameObject.SetActive(true); // Activate the fade image
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsed / duration); // Increase alpha
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1f; // Ensure it's fully opaque
        fadeImage.color = color;
    }

    public IEnumerator FadeIn(float duration)
    {
        float elapsed = 0f;
        Color color = fadeImage.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            color.a = Mathf.Lerp(1f, 0f, elapsed / duration); // Decrease alpha
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0f; // Ensure it's fully transparent
        fadeImage.color = color;

        fadeImage.gameObject.SetActive(false); // Deactivate the fade image
    }
}
