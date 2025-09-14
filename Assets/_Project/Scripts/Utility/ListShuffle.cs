using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Utility
{
    /// <summary>
    /// Provides methods for shuffling the elements of a list or array in random order.
    /// </summary>
    /// <remarks>This class implements the Fisher-Yates shuffle algorithm to randomize the order of elements.
    /// It includes overloads for both <see cref="List{T}"/> and arrays, allowing flexibility in usage.</remarks>
    public static class ListShuffle
    {
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
            System.Random random = new System.Random();

            int n = array.Length;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = array[k];
                array[k] = array[n];
                array[n] = value;
            }
        }
    }
}