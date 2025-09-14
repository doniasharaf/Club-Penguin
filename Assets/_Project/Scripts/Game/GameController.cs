using Audio;
using Data;
using Persistence;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utility;

namespace GamePlay
{
    /// <summary>
    /// Main controller orchestrating the memory game flow:
    /// Initializes deck and grid
    /// Tracks score, streaks, and matches via GameEvaluator
    /// Handles saving/loading game state
    /// Notifies UI and plays sound effects
    /// </summary>
    public class GameController : MonoBehaviour
    {
        public UnityAction GameEnded;
        public UnityAction GameStarted;


        [Header("Data Model")]
        [SerializeField] private CardsData cardsData;

        [Header("View")]
        [SerializeField] private GridController gridController; //view
        [SerializeField] private GameObject cardPrefab; //view
        [SerializeField] private GameStatsUI statsUI; //view

        [Header("Progress Saving")]
        [SerializeField] private PersistenceManager saveLoadManager;

        [Header("Sound Manager")]
        [SerializeField] private SoundManager soundManager;


        private List<CardBehavior> _currentCards;
        private GameEvaluator _gameEvaluator;
        private int _rows;
        private int _columns;
        private bool _isGameActive;

        private const string PreviousGameKey = "previousGameExists";
        private const string SaveGameKey = "lastGame";

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
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus && _isGameActive)
            {
                SaveGame();
            }
        }

        private void OnApplicationQuit()
        {
            if (_isGameActive)
            {
                SaveGame();
            }
        }
        #region Public 
        /// <summary>
        /// Starts a new game session with the given grid size.
        /// </summary>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
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
            _currentCards.Clear();
            _gameEvaluator.Reset();
            gridController.SetGridSize(rows, columns);
            statsUI.ResetStats();
            List<CardData> deck = InitializeDeck(totalCards);
            CreateCards(deck);
            _isGameActive = true;
            GameStarted?.Invoke();
        }

        /// <summary>
        /// Persists the current game state to disk.
        /// </summary>
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
            saveLoadManager.SaveData<GameState>(SaveGameKey, gameState);
            PlayerPrefs.SetInt(PreviousGameKey, 1);
        }

        /// <summary>
        /// Attempts to load the most recent saved game.
        /// </summary>
        public void LoadGame()
        {
            if (saveLoadManager == null)
            {
                Debug.LogWarning("SaveLoadManager is not assigned, cannot load game.");
                return;
            }
            GameState previousGame = saveLoadManager.LoadData<GameState>(SaveGameKey);
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

        /// <summary>
        /// Restores a previously saved game state.
        /// </summary>
        /// <param name="gameState"></param>
        private void RestoreGame(GameState gameState)
        {
            _currentCards.Clear();
            _gameEvaluator.RestoreGame(gameState);
            _rows = gameState.Rows;
            _columns = gameState.Columns;
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
            _isGameActive = true;
            GameStarted?.Invoke();
        }

        private void EndGame()
        {
            soundManager.PlayGameOverSFX();
            PlayerPrefs.SetInt(PreviousGameKey, 0);
            _isGameActive = false;
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
            soundManager.PlayFlipSFX();
            _gameEvaluator.Evaluate(clickedCard);
        }

        private void OnMatchChecked(bool isMatched, CardBehavior cardOne, CardBehavior cardTwo)
        {
            if (isMatched)
            {
                soundManager.PlayMatchSFX();
                cardOne.DeactivateCard();
                cardTwo.DeactivateCard();

                if (_gameEvaluator.MatchedCards.Count >= _currentCards.Count)
                {
                    EndGame();
                }
            }
            else
            {
                soundManager.PlayMismatchSFX();
                StartCoroutine(HideMismatchedCards(cardOne, cardTwo));
            }
        }

        private void OnScoreChanged(int newScore)
        {
            statsUI.UpdateScore(newScore);
        }

        private void OnHighScoreChanged(int newHighScore)
        {
            statsUI.UpdateHighScore(newHighScore);
        }

        private void OnStreakChanged(int newStreak)
        {

            statsUI.UpdateStreak(newStreak);
        }
        #endregion
    }



}