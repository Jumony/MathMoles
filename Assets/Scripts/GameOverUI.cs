using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;

    private void Start()
    {
        DisplayScore();
    }

    private void DisplayScore()
    {
        var finalScore = GameController.FinalScore;
        scoreText.text = "Score: " + finalScore;
        Debug.Log("Previous high score: " + LeaderboardManager.Instance.GetPreviousHighScore());

        // Check if score was saved
        var scoreSaved = PlayerNameManager.Instance != null && PlayerNameManager.Instance.HasValidName();

        var manager = LeaderboardManager.Instance;
        if (manager == null) return;

        if (scoreSaved && finalScore > manager.GetPreviousHighScore() && finalScore > 0)
        {
            highScoreText.text = "NEW HIGH SCORE!";
            highScoreText.gameObject.SetActive(true);
        }
        else
        {
            highScoreText.gameObject.SetActive(false);
            Debug.Log(!scoreSaved ? "Score not saved - no name entered" : "Score saved to leaderboard");
        }
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
