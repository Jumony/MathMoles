using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class MainMenuManager : MonoBehaviour
    {
        public GameObject mainMenu;
        private GameObject _currentPanel;

        [Header("Name Input")]
        [SerializeField] private TMP_InputField nameInputField;

        [Header("Leaderboard")]
        [SerializeField] private LeaderboardUI leaderboardUI;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        public void Start()
        {
            _currentPanel = mainMenu;
            _currentPanel.SetActive(true);

            // Clear any previous player name when returning to the main menu
            if (PlayerNameManager.Instance != null)
            {
                PlayerNameManager.Instance.ClearPlayerName();
            }

            // Refresh leaderboard display
            if (leaderboardUI != null)
            {
                leaderboardUI.RefreshLeaderboard();
            }
        }

        public void ShowPanel(GameObject panel)
        {
            _currentPanel.SetActive(false);
            _currentPanel = panel;
            _currentPanel.SetActive(true);
        }

        public void SwitchScene(string scene)
        {
            // Save the player name if provided before switching to a game scene
            if (nameInputField != null && PlayerNameManager.Instance != null)
            {
                var enteredName = nameInputField.text.Trim();
                if (!string.IsNullOrWhiteSpace(enteredName))
                {
                    PlayerNameManager.Instance.SetPlayerName(enteredName);
                    Debug.Log($"Player name set to: {enteredName}");
                }
                else
                {
                    Debug.Log("No name entered - score will not be saved");
                }
            }

            SceneManager.LoadScene(scene);
        }

        public void OnClearLeaderboardClicked()
        {
            if (LeaderboardManager.Instance == null) return;
            LeaderboardManager.Instance.ClearLeaderboard();

            if (leaderboardUI != null)
            {
                leaderboardUI.RefreshLeaderboard();
            }
        }
    }
}
