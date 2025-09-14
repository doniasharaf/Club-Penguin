using TMPro;
using UnityEngine;

public class GameStatsUI : MonoBehaviour
{
    [Header("Game UI")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI streakText;

    private void Start()
    {
        ResetStats();
    }

    public void ResetStats()
    {
        UpdateScore(0);
        int highScore = PlayerPrefs.GetInt("highScore", 0);
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
