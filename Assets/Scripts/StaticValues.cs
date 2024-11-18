using UnityEngine;

public class StaticValues : MonoBehaviour
{
    private const string HighScoreKey = "HighScore";
    private const string GemKey = "Gems";

    public static int HighScore
    {
        get => PlayerPrefs.GetInt(HighScoreKey, 0);
        set
        {
            PlayerPrefs.SetInt(HighScoreKey, value);
            PlayerPrefs.Save();
        }
    }

        public static int Gems
    {
        get => PlayerPrefs.GetInt(GemKey, 0);
        set
        {
            PlayerPrefs.SetInt(GemKey, value);
            PlayerPrefs.Save();
        }
    }
}
