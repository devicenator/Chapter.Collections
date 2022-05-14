// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable once CheckNamespace

namespace SniffCore.Collections
{
    /// <summary>
    ///     Extends a <see cref="List{T}" /> or <see cref="IList{T}" /> with useful methods.
    /// </summary>
    /// <example>
    ///     <code lang="csharp">
    /// <![CDATA[
    /// // Gets the position of the first matched element
    /// var index = items.IndexOf(x => Matches(x));
    /// 
    /// // Gets the position of the last matched element
    /// var index = items.LastIndexOf(x => Matches(x));
    /// 
    /// // Splits the list in multiple list with 10 items each
    /// var batches = items.Split(10);
    /// 
    /// // Shuffles the items
    /// items = items.Shuffle();
    /// ]]>
    /// </code>
    /// </example>
    public static class ListEx
    {
        /// <summary>
        ///     Gets the index of the first element matched by a condition.
        /// </summary>
        /// <typeparam name="T">The inner type of the list.</typeparam>
        /// <param name="list">The list to gets the index from.</param>
        /// <param name="condition">The match condition.</param>
        /// <returns>The index of the first matched item; otherwise -1.</returns>
        /// <exception cref="ArgumentNullException">list is null.</exception>
        /// <exception cref="ArgumentNullException">condition is null.</exception>
        public static int IndexOf<T>(this IList<T> list, Func<T, bool> condition)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            for (var i = 0; i < list.Count; ++i)
                if (condition(list[i]))
                    return i;

            return -1;
        }

        /// <summary>
        ///     Gets the index of the last element matched by a condition.
        /// </summary>
        /// <typeparam name="T">The inner type of the list.</typeparam>
        /// <param name="list">The list to gets the index from.</param>
        /// <param name="condition">The match condition.</param>
        /// <returns>The index of the last matched item; otherwise -1.</returns>
        /// <exception cref="ArgumentNullException">list is null.</exception>
        /// <exception cref="ArgumentNullException">condition is null.</exception>
        public static int LastIndexOf<T>(this IList<T> list, Func<T, bool> condition)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            for (var i = list.Count - 1; i >= 0; --i)
                if (condition(list[i]))
                    return i;
            return -1;
        }

        /// <summary>
        ///     Splits the list into multiple lists with a given amount for each.
        /// </summary>
        /// <remarks>The last list can be shorter than the amount.</remarks>
        /// <typeparam name="T">The inner type of the list.</typeparam>
        /// <param name="list">The list to split.</param>
        /// <param name="amounts">The maximum amount of items in a list.</param>
        /// <returns>A list of lists with the items.</returns>
        /// <exception cref="ArgumentNullException">list is null.</exception>
        public static List<List<T>> Split<T>(this List<T> list, uint amounts)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

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
        /// <exception cref="ArgumentNullException">list is null.</exception>
        public static List<T> Shuffle<T>(this List<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            return list.OrderBy(x => Guid.NewGuid()).ToList();
        }

        /// <summary>
        ///     Shuffles the items in the list into a new list.
        /// </summary>
        /// <typeparam name="T">The inner type of the list.</typeparam>
        /// <param name="list">The list to shuffle.</param>
        /// <returns>The items shuffled into a new list.</returns>
        /// <exception cref="ArgumentNullException">list is null.</exception>
        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            return list.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}