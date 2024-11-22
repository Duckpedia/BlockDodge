using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour
{
    public TextMeshProUGUI GemsText;
    public Button ClaimButton50;
    public Button ClaimButton100;
    public GameObject AchievementsPanel;

    private int[] achievementScores = { 50, 100 };
    private bool[] achievementsClaimed;

    void Update()
    {
        if(!AchievementsPanel.activeSelf){
            HideClaimButtons();
        }
    }

    void Awake()
    {
        achievementsClaimed = new bool[achievementScores.Length];

        for (int i = 0; i < achievementsClaimed.Length; i++)
        {
            achievementsClaimed[i] = PlayerPrefs.GetInt($"AchievementClaimed_{achievementScores[i]}", 0) == 1;
        }

        UpdateGemsText();
        HideClaimButtons();
    }

    public void CheckForAchievements(int itemsdodged)
    {
        if (itemsdodged >= 50 && !achievementsClaimed[0])
        {
            ShowClaimButton(50);
        }

        if (itemsdodged >= 100 && !achievementsClaimed[1])
        {
            ShowClaimButton(100);
        }
    }

    public void ClaimRewardForScore50()
    {
        ClaimReward(0, 50);
    }

    public void ClaimRewardForScore100()
    {
        ClaimReward(1, 100);
    }

    private void ClaimReward(int index, int itemsdodged)
    {
        if (!achievementsClaimed[index])
        {
            achievementsClaimed[index] = true;
            StaticValues.Gems += 25;
            UpdateGemsText();
            HideClaimButton(itemsdodged);

            PlayerPrefs.SetInt($"AchievementClaimed_{itemsdodged}", 1);
            PlayerPrefs.Save();
        }
    }

    private void UpdateGemsText()
    {
        GemsText.text = StaticValues.Gems.ToString();
    }

    private void ShowClaimButton(int itemsdodged)
    {
        if (AchievementsPanel.activeSelf)
        {
            if (itemsdodged == 50 && !achievementsClaimed[0])
            {
                ClaimButton50.gameObject.SetActive(true);
            }
            else if (itemsdodged == 100 && !achievementsClaimed[1])
            {
                ClaimButton100.gameObject.SetActive(true);
            }
        }
    
    }

    private void HideClaimButtons()
    {
        ClaimButton50.gameObject.SetActive(false);
        ClaimButton100.gameObject.SetActive(false);
    }

    private void HideClaimButton(int itemsdodged)
    {
        if (itemsdodged == 50)
        {
            ClaimButton50.gameObject.SetActive(false);
        }
        else if (itemsdodged == 100)
        {
            ClaimButton100.gameObject.SetActive(false);
        }
    }

    public void OnAchievementsPanelOpened(int itemsdodged)
    {
        CheckForAchievements(itemsdodged);
    }
}
