using UnityEngine;

public class StaticValues : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";

    public static int HighScore
    {
        get => PlayerPrefs.GetInt(HighScoreKey, 0);
        set
        {
            PlayerPrefs.SetInt(HighScoreKey, value);
            PlayerPrefs.Save();
        }
    }
}
