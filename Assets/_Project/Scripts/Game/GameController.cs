using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Data Model")]
    [SerializeField] private CardsData cardsData;

    [Header("View")]
    [SerializeField] private GridController gridController; //view
    [SerializeField] private GameObject cardPrefab; //view

    private List<CardBehavior> currentCards = new List<CardBehavior>();


    public void StartGame(int rows, int columns)
    {
        int totalCards = rows * columns;
        if (totalCards % 2 != 0)
        {
            Debug.LogError("Total number of cards must be even.");
            return;
        }
        gridController.SetGridSize(rows, columns);
        List<CardData> deck = InitializeDeck(totalCards);
        CreateCards(deck);
    }


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
            currentCards.Add(newCard);
            newCard.CardID = cardData.CardId;
            newCard.SetCardImage(cardData.CardSprite);
            newCard.CardClicked += OnCardClicked;
            gridController.AddChildTransform(newCard.transform);
        }
    }

    private void OnCardClicked(CardBehavior clickedCard)
    {

    }

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

public static class ListShuffle
{
    //Fisher-Yates shuffle algorithm
    public static void Shuffle<T>(List<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public static void Shuffle<T>(T[] array)
    {
        // Use System.Random for general-purpose shuffling, or UnityEngine.Random for game-specific randomness.
        System.Random random = new System.Random();

        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1); // Generate a random index from 0 to n (inclusive)
            T value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
    }
}

