using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AchievementsManager : MonoBehaviour
{
    public TextMeshProUGUI GemsText;
    public Button ClaimButton50; // Changed from 50 to 20
    public Button ClaimButton100;
    public GameObject AchievementsPanel;

    private int[] achievementScores = { 50, 100 }; // Change 50 to 20
    private bool[] achievementsClaimed;

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

    public void CheckForAchievements(int score, int HighScore)
    {
        // Check for achievements and display buttons
        if (score >= 50 && !achievementsClaimed[0] || HighScore >= 50)
        {
            ShowClaimButton(50); // Changed 50 to 20
        }

        if (score >= 100 && !achievementsClaimed[1] || HighScore >= 100)
        {
            ShowClaimButton(100);
        }
    }

    public void ClaimRewardForScore50() // Changed from 50 to 20
    {
        ClaimReward(0, 50); // Changed from 50 to 20
    }

    public void ClaimRewardForScore100()
    {
        ClaimReward(1, 100);
    }

    private void ClaimReward(int index, int score)
    {
        if (!achievementsClaimed[index]) // Can only claim if not already claimed
        {
            achievementsClaimed[index] = true;
            StaticValues.Gems += 25; // Add gems to the player
            UpdateGemsText();
            HideClaimButton(score);

            PlayerPrefs.SetInt($"AchievementClaimed_{score}", 1); // Save the claim status
            PlayerPrefs.Save(); // Save changes to PlayerPrefs
        }
    }

    private void UpdateGemsText()
    {
        GemsText.text = StaticValues.Gems.ToString();
    }

    private void ShowClaimButton(int score)
    {
        if (AchievementsPanel.activeSelf) // Only show when the panel is active
        {
            if (score == 50 && !achievementsClaimed[0]) // Check for 20 score
            {
                ClaimButton50.gameObject.SetActive(true);
            }
            else if (score == 100 && !achievementsClaimed[1]) // Check for 100 score
            {
                ClaimButton100.gameObject.SetActive(true);
            }
        }
    }

    private void HideClaimButtons()
    {
        ClaimButton50.gameObject.SetActive(false); // Hide 20 button
        ClaimButton100.gameObject.SetActive(false); // Hide 100 button
    }

    private void HideClaimButton(int score)
    {
        if (score == 50)
        {
            ClaimButton50.gameObject.SetActive(false);
        }
        else if (score == 100)
        {
            ClaimButton100.gameObject.SetActive(false);
        }
    }

    // Call this method when opening the achievements panel
    public void OnAchievementsPanelOpened(int score, int HighScore)
    {
        CheckForAchievements(score, HighScore);
    }
}
