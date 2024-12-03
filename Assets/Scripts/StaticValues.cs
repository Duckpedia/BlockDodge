using UnityEngine;

public class StaticValues : MonoBehaviour
{
    private const string GemKey = "Gems";
    private const string StoryKey = "Story";

        public static int Gems
    {
        get => PlayerPrefs.GetInt(GemKey, 0);
        set
        {
            PlayerPrefs.SetInt(GemKey, value);
            PlayerPrefs.Save();
        }
    }

            public static int StoryCompleted
    {
        get => PlayerPrefs.GetInt(StoryKey, 0);
        set
        {
            PlayerPrefs.SetInt(StoryKey, value);
            PlayerPrefs.Save();
        }
    }
}
