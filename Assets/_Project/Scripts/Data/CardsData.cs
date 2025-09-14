using UnityEngine;

namespace Data
{
    /// <summary>
    /// ScriptableObject container for a collection of cards used in the game.
    /// Provides card definitions (ID and sprite) that can be referenced at runtime.
    /// </summary>
    [CreateAssetMenu(fileName = "CardsData", menuName = "Data/CardsData")]
    public class CardsData : ScriptableObject
    {
        public CardData[] Cards;
    }
    [System.Serializable]
    public struct CardData
    {
        public int CardId;
        public Sprite CardSprite;
    }
}