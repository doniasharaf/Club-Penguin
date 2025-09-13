using UnityEngine;

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