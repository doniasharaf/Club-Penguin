using System.Collections.Generic;
using UnityEngine.Events;

[System.Serializable]
public class GameEvaluator
{
    public UnityAction<bool, CardBehavior, CardBehavior> MatchChecked;
    public UnityAction<int> ScoreUpdated;
    private List<CardBehavior> _clickedCards;
    public HashSet<CardBehavior> MatchedCards { get; set; }

    public GameEvaluator()
    {
        _clickedCards = new List<CardBehavior>();
        MatchedCards = new HashSet<CardBehavior>();
        Score = 0;
        ScoreUpdated?.Invoke(Score);
    }
    public int Score { get; private set; }

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
            Score++;
            ScoreUpdated?.Invoke(Score);
            MatchedCards.Add(firstCard);
            MatchedCards.Add(secondCard);
            isMatched = true;
        }
        else
        {
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
        _clickedCards.Clear();
        MatchedCards.Clear();
    }

    public void RestoreGame(GameState gameState)
    {
        _clickedCards.Clear();
        MatchedCards.Clear();
        Score = gameState.Score;
        ScoreUpdated?.Invoke(Score);
    }

}
