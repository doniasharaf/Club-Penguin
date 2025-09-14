using Data;
using System.Collections.Generic;

namespace Persistence
{
    /// <summary>
    /// Represents the current state of the game, including the grid dimensions, score, streak, and the state of all
    /// cards.
    /// </summary>
    /// <remarks>This class is used to encapsulate all relevant information about the game's progress and
    /// state at a given point in time. It can be serialized to save or transfer the game state.</remarks>
    [System.Serializable]
    public class GameState
    {
        public int Rows;
        public int Columns;
        public int Score;
        public int Streak;
        public List<CardState> CardStates;
    }

    [System.Serializable]
    public class CardState
    {
        public CardData Data;
        public bool IsMatched;
    }
}