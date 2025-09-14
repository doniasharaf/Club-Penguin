using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvaluator
{
    public UnityAction<bool, CardBehavior, CardBehavior> MatchChecked;
    public UnityAction<int> ScoreUpdated;
    public UnityAction<int> HighScoreUpdated;
    public UnityAction<int> StreakUpdated;

    private List<CardBehavior> _clickedCards;
    private int _highScore;
    public int Score { get; private set; }
    public int Streak { get; private set; }

    public HashSet<CardBehavior> MatchedCards { get; set; }

    public GameEvaluator()
    {
        _clickedCards = new List<CardBehavior>();
        MatchedCards = new HashSet<CardBehavior>();
        Streak = 0;
        Score = 0;
        ScoreUpdated?.Invoke(Score);
        _highScore = PlayerPrefs.GetInt("highScore", 0);
    }

    public void Evaluate(CardBehavior clickedCard)
    {
        _clickedCards.Add(clickedCard);
        if (_clickedCards.Count >= 2)
        {
            var firstCard = _clickedCards[0];
            var secondCard = _clickedCards[1];
            bool isMatch = CheckMatch(firstCard, secondCard);
            _clickedCards.Remove(firstCard);
            _clickedCards.Remove(secondCard);
        }
    }

    public bool CheckMatch(CardBehavior firstCard, CardBehavior secondCard)
    {
        bool isMatched;
        if (firstCard.Data.CardId == secondCard.Data.CardId)
        {
            Streak++;
            Score += Streak;
            ScoreUpdated?.Invoke(Score);
            if (Streak > 1)
                StreakUpdated?.Invoke(Streak);
            if (Score > _highScore)
            {
                _highScore = Score;
                PlayerPrefs.SetInt("highScore", _highScore);
                HighScoreUpdated?.Invoke(_highScore);
            }
            MatchedCards.Add(firstCard);
            MatchedCards.Add(secondCard);
            isMatched = true;
        }
        else
        {
            Streak = 0;
            StreakUpdated?.Invoke(Streak);
            isMatched = false;
        }
        MatchChecked?.Invoke(isMatched, firstCard, secondCard);
        return isMatched;
    }
    public bool IsCardMatched(CardBehavior card)
    {
        return MatchedCards.Contains(card);
    }

    public void Reset()
    {
        Score = 0;
        ScoreUpdated?.Invoke(Score);
        Streak = 0;
        _clickedCards.Clear();
        MatchedCards.Clear();
    }

    public void RestoreGame(GameState gameState)
    {
        _clickedCards.Clear();
        MatchedCards.Clear();
        Score = gameState.Score;
        Streak = gameState.Streak;
        ScoreUpdated?.Invoke(Score);
    }

}
