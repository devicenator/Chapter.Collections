//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace SniffCore.Collections
{
    /// <summary>
    ///     Extends a <see cref="List{T}" /> or <see cref="IList{T}" /> with useful methods.
    /// </summary>
    public static class ListEx
    {
        /// <summary>
        ///     Gets the index of the first element matched by a condition.
        /// </summary>
        /// <typeparam name="T">The inner type of the list.</typeparam>
        /// <param name="list">The list to gets the index from.</param>
        /// <param name="condition">The match condition.</param>
        /// <returns>The index of the first matched item; otherwise -1.</returns>
        /// <exception cref="ArgumentNullException">condition is null.</exception>
        public static int IndexOf<T>(this IList<T> list, Func<T, bool> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            var item = list.FirstOrDefault(condition);
            if (Equals(item, null))
                return -1;

            return list.IndexOf(item);
        }

        /// <summary>
        ///     Gets the index of the last element matched by a condition.
        /// </summary>
        /// <typeparam name="T">The inner type of the list.</typeparam>
        /// <param name="list">The list to gets the index from.</param>
        /// <param name="condition">The match condition.</param>
        /// <returns>The index of the last matched item; otherwise -1.</returns>
        /// <exception cref="ArgumentNullException">condition is null.</exception>
        public static int LastIndexOf<T>(this IList<T> list, Func<T, bool> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            var item = list.LastOrDefault(condition);
            if (Equals(item, null))
                return -1;

            return list.IndexOf(item);
        }

        /// <summary>
        ///     Splits the list into multiple lists with a given amount for each.
        /// </summary>
        /// <remarks>The last list can be shorter than the amount.</remarks>
        /// <typeparam name="T">The inner type of the list.</typeparam>
        /// <param name="list">The list to split.</param>
        /// <param name="amounts">The maximum amount of items in a list.</param>
        /// <returns>A list of lists with the items.</returns>
        public static List<List<T>> Split<T>(this List<T> list, uint amounts)
        {
            if (amounts == 0)
                throw new ArgumentException("amounts cannot be 0", nameof(amounts));

            var amount = (int) amounts;
            var lists = new List<List<T>>();
            var index = 0;
            while (index < list.Count)
            {
                var count = amount;
                if (list.Count - index <= amounts)
                    count = list.Count - index;

                lists.Add(list.GetRange(index, count));
                index += amount;
            }

            return lists;
        }

        /// <summary>
        ///     Shuffles the items in the list into a new list.
        /// </summary>
        /// <typeparam name="T">The inner type of the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        /// <returns>The items shuffled into a new list.</returns>
        public static List<T> Shuffle<T>(this List<T> list)
        {
            return list.OrderBy(x => Guid.NewGuid()).ToList();
        }

        /// <summary>
        ///     Shuffles the items in the list into a new list.
        /// </summary>
        /// <typeparam name="T">The inner type of the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        /// <returns>The items shuffled into a new list.</returns>
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            return list.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}