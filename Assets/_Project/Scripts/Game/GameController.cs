using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    public UnityAction GameEnded;

    [Header("Data Model")]
    [SerializeField] private CardsData cardsData;

    [Header("View")]
    [SerializeField] private GridController gridController; //view
    [SerializeField] private GameObject cardPrefab; //view
    [SerializeField] private GameStatsUI statsUI; //view

    [Header("Progress Saving")]
    [SerializeField] private SaveLoadManager saveLoadManager;

    private List<CardBehavior> _currentCards;
    private GameEvaluator _gameEvaluator;
    private int _rows;
    private int _columns;

    private void Awake()
    {
        if (saveLoadManager == null)
            Debug.LogWarning(" SaveLoadManager is not assigned, saving/loading will be disabled.");
        _gameEvaluator = new GameEvaluator();
        _gameEvaluator.MatchChecked += OnMatchChecked;
        _gameEvaluator.ScoreUpdated += OnScoreChanged;
        _gameEvaluator.HighScoreUpdated += OnHighScoreChanged;
        _gameEvaluator.StreakUpdated += OnStreakChanged;
        _currentCards = new List<CardBehavior>();
    }


    #region Public 
    public void StartGame(int rows, int columns)
    {
        _rows = rows;
        _columns = columns;
        int totalCards = rows * columns;
        if (totalCards % 2 != 0)
        {
            Debug.LogError("Total number of cards must be even.");
            return;
        }
        gridController.SetGridSize(rows, columns);
        statsUI.ResetStats();
        List<CardData> deck = InitializeDeck(totalCards);
        CreateCards(deck);
    }


    [ContextMenu("Save Game")]
    public void SaveGame()
    {
        if (saveLoadManager == null)
        {
            Debug.LogWarning("SaveLoadManager is not assigned, cannot save game.");
            return;
        }
        GameState gameState = new GameState();
        gameState.Rows = _rows;
        gameState.Columns = _columns;
        gameState.Score = _gameEvaluator.Score;
        gameState.Streak = _gameEvaluator.Streak;
        gameState.CardStates = new List<CardState>();
        foreach (CardBehavior card in _currentCards)
        {
            gameState.CardStates.Add(new CardState
            {
                Data = card.Data,
                IsMatched = _gameEvaluator.IsCardMatched(card)
            });
        }
        saveLoadManager.SaveData<GameState>("lastGame", gameState);
        PlayerPrefs.SetInt("previousGameExists", 1);
    }

    [ContextMenu("Load Game")]
    public void LoadGame()
    {
        if (saveLoadManager == null)
        {
            Debug.LogWarning("SaveLoadManager is not assigned, cannot load game.");
            return;
        }
        GameState previousGame = saveLoadManager.LoadData<GameState>("lastGame");
        if (previousGame != null)
            RestoreGame(previousGame);
    }

    #endregion

    #region Private
    private List<CardData> InitializeDeck(int totalCards)
    {
        //Shuffle available card definitions
        int pairs = totalCards / 2;
        ListShuffle.Shuffle<CardData>(cardsData.Cards);

        //Select required number of pairs
        List<CardData> selectedCards = new List<CardData>();
        for (int i = 0; i < pairs; i++)
        {
            selectedCards.Add(cardsData.Cards[i % cardsData.Cards.Length]);
        }

        //create two of every selected card in the game deck
        List<CardData> deck = new List<CardData>();
        foreach (CardData card in selectedCards)
        {
            deck.Add(card);
            deck.Add(card);
        }

        ListShuffle.Shuffle<CardData>(deck);

        return deck;

    }

    private void CreateCards(List<CardData> deck)
    {
        foreach (CardData cardData in deck)
        {
            GameObject newCardObject = Instantiate(cardPrefab);
            CardBehavior newCard = newCardObject.GetComponent<CardBehavior>();
            _currentCards.Add(newCard);
            newCard.Data = cardData;
            newCard.CardClicked += OnCardClicked;
            gridController.AddChildTransform(newCard.transform);
        }
    }
    private void RestoreGame(GameState gameState)
    {
        foreach (CardBehavior card in _currentCards)
        {
            Destroy(card.gameObject);
        }
        _currentCards.Clear();
        _gameEvaluator.RestoreGame(gameState);
        _rows = gameState.Rows;
        _columns = gameState.Columns;
        statsUI.ResetStats();
        gridController.SetGridSize(_rows, _columns);
        foreach (CardState cardState in gameState.CardStates)
        {
            GameObject newCardObject = Instantiate(cardPrefab);
            CardBehavior newCard = newCardObject.GetComponent<CardBehavior>();
            newCard.Data = cardState.Data;

            if (cardState.IsMatched)
            {
                newCard.ShowCard();
                newCard.DeactivateCard();
                _gameEvaluator.MatchedCards.Add(newCard);
            }
            else
            {
                newCard.CardClicked += OnCardClicked;
                newCard.HideCard();
            }

            _currentCards.Add(newCard);
            gridController.AddChildTransform(newCard.transform);
        }
    }

    private void EndGame()
    {
        Debug.Log($"Game Over! Final Score: {_gameEvaluator.Score}, High Score: {PlayerPrefs.GetInt("highScore")}");
        GameEnded?.Invoke();
    }

    private IEnumerator HideMismatchedCards(CardBehavior cardOne, CardBehavior cardTwo)
    {
        yield return new WaitForSeconds(1f);
        cardOne.HideCard();
        cardTwo.HideCard();

    }
    #endregion

    #region Callbacks

    private void OnCardClicked(CardBehavior clickedCard)
    {
        _gameEvaluator.Evaluate(clickedCard);
    }

    private void OnMatchChecked(bool isMatched, CardBehavior cardOne, CardBehavior cardTwo)
    {
        if (isMatched)
        {
            cardOne.DeactivateCard();
            cardTwo.DeactivateCard();

            if (_gameEvaluator.MatchedCards.Count >= _currentCards.Count)
            {
                EndGame();
            }
        }
        else
        {
            StartCoroutine(HideMismatchedCards(cardOne, cardTwo));
        }
    }

    private void OnScoreChanged(int newScore)
    {
        Debug.Log($"Score updated: {newScore}");
        statsUI.UpdateScore(newScore);
    }

    private void OnHighScoreChanged(int newHighScore)
    {
        Debug.Log($"High Score updated: {newHighScore}");
        statsUI.UpdateHighScore(newHighScore);
    }

    private void OnStreakChanged(int newStreak)
    {

        Debug.Log($"Streak updated: {newStreak}");
        statsUI.UpdateStreak(newStreak);
    }
    #endregion

#if UNITY_EDITOR
    [ContextMenu("Start 4x4 Game")]
    public void Start4x4Game()
    {
        StartGame(4, 4);
    }
    [ContextMenu("Start 5x6 Game")]
    public void Start5x6Game()
    {
        StartGame(5, 6);
    }
#endif
}



