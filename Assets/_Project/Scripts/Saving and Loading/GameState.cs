using System.Collections.Generic;

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
