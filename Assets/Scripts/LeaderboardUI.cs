using TMPro;
using UnityEngine;

public class LeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform leaderboardContainer;
    [SerializeField] private GameObject leaderboardEntryPrefab;

    private void Start()
    {
        DisplayLeaderboard();
    }

    private void DisplayLeaderboard()
    {
        Debug.Log("DisplayLeaderboard called");
        if (LeaderboardManager.Instance == null)
        {
            Debug.LogWarning("LeaderboardManager not found!");
            return;
        }

        var scores = LeaderboardManager.Instance.GetLeaderboard();

        // Method 1: Using prefabs for each entry
        if (leaderboardContainer == null || leaderboardEntryPrefab == null) return;
        // Clear existing entries
        foreach (Transform child in leaderboardContainer)
        {
            Destroy(child.gameObject);
        }

        // Create new entries
        for (var i = 0; i < scores.Count; i++)
        {
            var entry = Instantiate(leaderboardEntryPrefab, leaderboardContainer);
            var text = entry.GetComponentInChildren<TextMeshProUGUI>();

            if (text != null)
            {
                text.text = $"{i + 1}. {scores[i].playerName} - {scores[i].score}";
            }
        }
    }

    // Call this if you want to refresh the leaderboard display
    public void RefreshLeaderboard()
    {
        DisplayLeaderboard();
    }
}
