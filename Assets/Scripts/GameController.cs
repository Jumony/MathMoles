using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using System.Collections;
using System.Globalization;
using Enemy;
using UnityEngine.SceneManagement;
using UI;

public class GameController : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField inputField;
    public TMP_Text streakText;
    public TMP_Text timerText;
    public TMP_Text scoreText;
    public GameObject feedbackText;
    public GameMenuManager gameMenuManager;
    public GameObject pauseMenu;
    public GameObject gameMenu;


    [Header("Game Objects")]
    public GameObject hammerPrefab;
    public List<Creature> activeCreatures = new();

    [Header("Game Settings")]
    public float gameDuration = 30f;
    public int pointsPerCorrectAnswer = 10;
    public int streakBonusMultiplier = 2;
    public float hammerDisplayDuration = 0.5f;
    public Vector3 hammerOffset = new(0, 1, 0);

    private string _currentInput = "";
    private int _streak;
    private int _score;
    public static int FinalScore { get; private set; }
    private float _timeRemaining;
    private bool _gameRunning = true;
    private bool _isPaused;

    private void Start()
    {
        Time.timeScale = 1f;
        inputField.ActivateInputField();
        _streak = 0;
        _timeRemaining = gameDuration;
        timerText.text = Mathf.CeilToInt(_timeRemaining).ToString();
        inputField.contentType = TMP_InputField.ContentType.IntegerNumber; // For only integers

    }

    private void Update()
    {

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (!_isPaused)
            {
                gameMenuManager.ShowPanel(pauseMenu);
                gameMenuManager.PauseGame();
                _isPaused = true;
            }

            else
            {
                gameMenuManager.ShowPanel(gameMenu);
                gameMenuManager.UnpauseGame(gameMenu);
                _isPaused = false;
            }
        }

        if (_gameRunning && !_isPaused)
        {
            _timeRemaining -= Time.deltaTime;
            timerText.text = Mathf.CeilToInt(_timeRemaining).ToString();

            if (_timeRemaining <= 0)
            {
                EndGame();
                return;
            }
        }

        // Debug.Log("Game running: " + gameRunning);
        // Debug.Log("Is Paused: " + isPaused);
        if (!_gameRunning || _isPaused) return;
        if (Keyboard.current.digit0Key.wasPressedThisFrame) { AppendToInput(0.ToString()); }
        if (Keyboard.current.digit1Key.wasPressedThisFrame) { AppendToInput(1.ToString()); }
        if (Keyboard.current.digit2Key.wasPressedThisFrame) { AppendToInput(2.ToString()); }
        if (Keyboard.current.digit3Key.wasPressedThisFrame) { AppendToInput(3.ToString()); }
        if (Keyboard.current.digit4Key.wasPressedThisFrame) { AppendToInput(4.ToString()); }
        if (Keyboard.current.digit5Key.wasPressedThisFrame) { AppendToInput(5.ToString()); }
        if (Keyboard.current.digit6Key.wasPressedThisFrame) { AppendToInput(6.ToString()); }
        if (Keyboard.current.digit7Key.wasPressedThisFrame) { AppendToInput(7.ToString()); }
        if (Keyboard.current.digit8Key.wasPressedThisFrame) { AppendToInput(8.ToString()); }
        if (Keyboard.current.digit9Key.wasPressedThisFrame) { AppendToInput(9.ToString()); }
        if (Keyboard.current.minusKey.wasPressedThisFrame)
        {
            if (_currentInput.Length == 0)
            {
                _currentInput += "-";
                inputField.text = _currentInput;
            }
        }
        if (Keyboard.current.backspaceKey.wasPressedThisFrame)
        {
            RemoveLastCharacter();
        }

        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SubmitInput();
        }
    }

    private void AppendToInput(string number)
    {
        _currentInput += number;
        inputField.text = _currentInput;
    }

    private void RemoveLastCharacter()
    {
        if (_currentInput.Length <= 0) return;
        _currentInput = _currentInput[..^1];
        inputField.text = _currentInput;
    }

    private void SubmitInput()
    {
        Debug.Log("User submitted input: " + _currentInput);
        if (!float.TryParse(_currentInput, out var userInput)) return;
        for (var i = activeCreatures.Count - 1; i >= 0; i--)
        {
            var creature = activeCreatures[i];

            if (!Mathf.Approximately(creature.solution, userInput)) continue;
            StartCoroutine(ShowCorrectText());

            _streak++;
            streakText.text = $"Streak: {_streak}";

            var earnedPoints = pointsPerCorrectAnswer + (_streak * streakBonusMultiplier);
            _score += earnedPoints;
            scoreText.text = $"Score: {_score}";

            StartCoroutine(HammerSmashCreature(creature));
                    
            activeCreatures.RemoveAt(i);
            inputField.text = "";
            _currentInput = "";
            return;
        }
            
        StartCoroutine(ShowIncorrectText());

        _streak = 0;
        streakText.text = _streak.ToString();
        inputField.text = "";
        _currentInput = "";
    }

    private IEnumerator HammerSmashCreature(Creature creature)
    {
        if (hammerPrefab != null && creature != null)
        {
            var hammerPosition = creature.transform.position + hammerOffset;
            var hammer = Instantiate(hammerPrefab, hammerPosition, Quaternion.identity);

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayHammerHit();
            }

            yield return new WaitForSeconds(hammerDisplayDuration);

            // Destroy both hammer and creature
            Destroy(hammer);
            if (creature != null)
            {
                Destroy(creature.gameObject);
            }
        }
        else
        {
            // If no hammer prefab, just destroy creature immediately
            if (creature != null)
            {
                Destroy(creature.gameObject);  
            }
        }
    }

    private void EndGame()
    {
        _gameRunning = false;
        _timeRemaining = 0;
        timerText.text = _timeRemaining.ToString(CultureInfo.InvariantCulture);

        inputField.interactable = false;

        FinalScore = _score;
        if (LeaderboardManager.Instance != null && PlayerNameManager.Instance != null)
        {
            if (PlayerNameManager.Instance.HasValidName())
            {
                var playerName = PlayerNameManager.Instance.GetPlayerName();
                LeaderboardManager.Instance.AddScore(_score, playerName);
                Debug.Log($"Score saved for {playerName}: {_score}");
            }
            else
            {
                Debug.Log("No name entered - score not saved to leaderboard");
            }
        }

        SceneManager.LoadScene("GameOverScene");
    }

    private IEnumerator ShowCorrectText()
    {
        feedbackText.GetComponent<TMP_Text>().text = "Correct!";
        feedbackText.SetActive(true);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCorrectAnswer();
        }

        yield return new WaitForSeconds(2);
        feedbackText.SetActive(false);
    }

    private IEnumerator ShowIncorrectText()
    {
        feedbackText.GetComponent<TMP_Text>().text = "Incorrect!";
        feedbackText.SetActive(true);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayIncorrectAnswer();
        }

        yield return new WaitForSeconds(2);
        feedbackText.SetActive(false);
    }
}