using TMPro;
using UnityEngine;

namespace GamePlay
{
    /// <summary>
    /// Handles updating the game statistics UI (score, high score, streak).
    /// Receives values from the GameController/Evaluator and reflects them visually.
    /// </summary>
    public class GameStatsUI : MonoBehaviour
    {
        [Header("Game UI")]
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI highScoreText;
        [SerializeField] private TextMeshProUGUI streakText;

        private const string HighScoreKey = "highScore";


        private void Start()
        {
            ResetStats();
        }

        /// <summary>
        /// Clears the UI stats to their default display values.
        /// </summary>
        public void ResetStats()
        {
            UpdateScore(0);
            int highScore = PlayerPrefs.GetInt(HighScoreKey, 0);
            UpdateHighScore(highScore);
            UpdateStreak(0);
        }
        public void UpdateScore(int score)
        {
            scoreText.text = $"{score}";
        }

        public void UpdateHighScore(int highScore)
        {
            highScoreText.text = $"{highScore}";
        }

        public void UpdateStreak(int streak)
        {
            if (streak > 1)
            {
                streakText.gameObject.SetActive(true);
                streakText.text = $"Streak!!!! x{streak}";
            }
            else
            {
                streakText.gameObject.SetActive(false);
                streakText.text = string.Empty;
            }
        }
    }
}