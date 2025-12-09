using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

namespace UI
{
    public class GameMenuManager : MonoBehaviour
    {
        public GameObject gameMenu;
        public GameObject pauseMenu;

        private GameObject _currentPanel;
        private bool isPaused = false;

        public void Start()
        {
            _currentPanel = gameMenu;
            _currentPanel.SetActive(true);

            // Make sure pause menu is hidden at start
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
        }

        /*
        void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (isPaused)
                    Resume();
                else
                    Pause();
            }
        }
        */
        public void Pause()
        {
            if (pauseMenu != null)
            {
                isPaused = true;
                pauseMenu.SetActive(true);
                Time.timeScale = 0f;
            }
        }

        public void Resume()
        {
            if (pauseMenu != null)
            {
                isPaused = false;
                pauseMenu.SetActive(false);
                Time.timeScale = 1f;
            }
        }

        public void ShowPanel(GameObject panel)
        {
            HideElement(_currentPanel);
            _currentPanel = panel;
            _currentPanel.SetActive(true);
        }

        public void ShowElement(GameObject element)
        {
            element.SetActive(true);
        }

        public void HideElement(GameObject element)
        {
            element.SetActive(false);
        }

        public void SwitchScene(string scene)
        {
            // IMPORTANT: Always reset time scale before switching scenes
            Time.timeScale = 1f;
            SceneManager.LoadScene(scene);
        }

        public void UnpauseGame(GameObject gameScene)
        {
            _currentPanel.SetActive(false);
            _currentPanel = gameMenu;
            _currentPanel.SetActive(true);
            Time.timeScale = 1f;
        }

        public void PauseGame()
        {
            Time.timeScale = 0f;
        }
    }
}