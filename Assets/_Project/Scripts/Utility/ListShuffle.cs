using System.Collections.Generic;
using Random = UnityEngine.Random;

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