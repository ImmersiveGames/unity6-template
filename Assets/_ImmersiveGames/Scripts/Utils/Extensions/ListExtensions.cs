using System;
using System.Collections.Generic;
using System.Linq;

namespace _ImmersiveGames.Scripts.Utils.Extensions {
    public static class ListExtensions {
        private static Random _rng;
        
        /// <summary>
        /// Determines whether a collection is null or has no elements
        /// without having to enumerate the entire collection to get a count.
        ///
        /// Uses LINQ's Any() method to determine if the collection is empty,
        /// so there is some GC overhead.
        /// </summary>
        /// <param name="list">List to evaluate</param>
        public static bool IsNullOrEmpty<T>(this IList<T> list) {
            return list == null || !list.Any();
        }

        /// <summary>
        /// Creates a new list that is a copy of the original list.
        /// </summary>
        /// <param name="list">The original list to be copied.</param>
        /// <returns>A new list that is a copy of the original list.</returns>
        public static List<T> Clone<T>(this IList<T> list) {
            var newList = new List<T>();
            foreach (T item in list) {
                newList.Add(item);
            }

            return newList;
        }

        /// <summary>
        /// Swaps two elements in the list at the specified indices.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <param name="indexA">The index of the first element.</param>
        /// <param name="indexB">The index of the second element.</param>
        public static void Swap<T>(this IList<T> list, int indexA, int indexB) {
            (list[indexA], list[indexB]) = (list[indexB], list[indexA]);
        }

        /// <summary>
        /// Shuffles the elements in the list using the Durstenfeld implementation of the Fisher-Yates algorithm.
        /// This method modifies the input list in-place, ensuring each permutation is equally likely, and returns the list for method chaining.
        /// Reference: http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
        /// </summary>
        /// <param name="list">The list to be shuffled.</param>
        /// <typeparam name="T">The type of the elements in the list.</typeparam>
        /// <returns>The shuffled list.</returns>
        public static IList<T> Shuffle<T>(this IList<T> list) {
            _rng ??= new Random();
            int count = list.Count;
            while (count > 1) {
                --count;
                int index = _rng.Next(count + 1);
                (list[index], list[count]) = (list[count], list[index]);
            }
            return list;
        }

        /// <summary>
        /// Filters a collection based on a predicate and returns a new list
        /// containing the elements that match the specified condition.
        /// </summary>
        /// <param name="source">The collection to filter.</param>
        /// <param name="predicate">The condition that each element is tested against.</param>
        /// <returns>A new list containing elements that satisfy the predicate.</returns>
        public static IList<T> Filter<T>(this IList<T> source, Predicate<T> predicate) {
            List<T> list = new List<T>();
            foreach (T item in source) {
                if (predicate(item)) {
                    list.Add(item);
                }
            }
            return list;
        }
        /// <summary>
        /// Returns a random item from the list
        /// </summary>
        public static T GetRandomItem<T>(this IList<T> array)
        {
            _rng ??= new Random();
            if (array == null)
                throw new IndexOutOfRangeException(
                    " [ array is null ] Cannot select a random item from null"
                );
            if (array.Count == 0)
                throw new IndexOutOfRangeException(
                    " [ array is empty ] Cannot select a random item from an empty array"
                );
            return array[_rng.Next(0, array.Count)];
        }
        /// <summary>
        /// Returns a specified number of random items from the list without duplicates
        /// </summary>
        public static List<T> GetUniqueRandomItems<T>(this IList<T> list, int count)
        {
            _rng ??= new Random();
            if (count > list.Count)
                throw new ArgumentException("Requested count is larger than list size");

            List<T> result = new List<T>();
            List<T> tempList = new List<T>(list);
            for (int i = 0; i < count; i++)
            {
                int index = _rng.Next(0, tempList.Count);
                result.Add(tempList[index]);
                tempList.RemoveAt(index);
            }
            return result;
        }
        /// <summary>
        /// Removes all null elements from the list
        /// </summary>
        public static IList<T> RemoveNulls<T>(this List<T> list)
            where T : class
        {
            list.RemoveAll(item => item == null);
            return list;
        }
        
        public static void RefreshWith<T>(this List<T> list, IEnumerable<T> items) {
            list.Clear();
            list.AddRange(items);
        }
    }
}
