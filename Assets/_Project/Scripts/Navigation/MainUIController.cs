using GamePlay;
using UnityEngine;
using UnityEngine.UI;
namespace Navigation
{
    /// <summary>
    /// Handles navigation between the main menu, grid options, gameplay, and game over screens.
    /// Subscribes to <see cref="GameController"/> events and updates button interactivity based on save data.
    /// </summary>
    public class MainUIController : MonoBehaviour
    {
        [Header("Main Menu")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private Button startButton;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button quitButton;

        [Header("Grid Options")]
        [SerializeField] private GameObject gridOptionsPanel;
        [SerializeField] private GridOptionButton[] gridOptionButtons;
        [SerializeField] private GameObject gamePanel;
        [SerializeField] private Button backButton;

        [Header("Game Over")]
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private Button mainMenuButton;

        [Header("Game Controller")]
        [SerializeField] private GameController gameController;

        private void Awake()
        {
            gameController.GameEnded += OnGameOver;
            gameController.GameStarted += OnGameStarted;
            CheckSavedGame();
            Subscribe();
        }

        private void OnDestroy()
        {
            // Clean up event subscriptions
            gameController.GameEnded -= OnGameOver;
            gameController.GameStarted -= OnGameStarted;

            startButton.onClick.RemoveListener(OnStartClicked);
            resumeButton.onClick.RemoveListener(OnResumeClicked);
            quitButton.onClick.RemoveListener(OnQuitClicked);
            mainMenuButton.onClick.RemoveListener(OnHomeClicked);
            backButton.onClick.RemoveListener(OnGamePaused);

            foreach (var button in gridOptionButtons)
            {
                button.GridOptionSelected -= gameController.StartGame;
            }
        }


        private void CheckSavedGame()
        {
            if (PlayerPrefs.GetInt("previousGameExists") == 1)
            {
                resumeButton.interactable = true;

            }
            else
            {
                resumeButton.interactable = false;
            }
        }

        private void Subscribe()
        {
            startButton.onClick.AddListener(OnStartClicked);
            quitButton.onClick.AddListener(OnQuitClicked);
            resumeButton.onClick.AddListener(OnResumeClicked);
            mainMenuButton.onClick.AddListener(OnHomeClicked);
            backButton.onClick.AddListener(OnGamePaused);
            foreach (var button in gridOptionButtons)
            {
                button.GridOptionSelected += gameController.StartGame;
            }
        }
        private void OnGameOver()
        {
            gameOverPanel.SetActive(true);
        }

        private void OnGameStarted()
        {
            mainMenuPanel.SetActive(false);
            gridOptionsPanel.SetActive(false);
            gamePanel.SetActive(true);
        }

        private void OnStartClicked()
        {
            mainMenuPanel.SetActive(false);
            gridOptionsPanel.SetActive(true);
        }

        private void OnResumeClicked()
        {
            gameController.LoadGame();
        }

        private void OnQuitClicked()
        {
            Application.Quit();
        }

        private void OnHomeClicked()
        {
            gamePanel.SetActive(false);
            gameOverPanel.SetActive(false);
            CheckSavedGame();
            mainMenuPanel.SetActive(true);
        }

        private void OnGamePaused()
        {
            gameController.SaveGame();
            OnHomeClicked();
        }
    }
}