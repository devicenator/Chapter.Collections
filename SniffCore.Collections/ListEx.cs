//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace SniffCore.Collections
{
    public static class ListEx
    {
        public static int IndexOf<T>(this IList<T> list, Func<T, bool> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            var item = list.FirstOrDefault(condition);
            if (Equals(item, null))
                return -1;

            return list.IndexOf(item);
        }

        public static int LastIndexOf<T>(this IList<T> list, Func<T, bool> condition)
        {
            if (condition == null)
                throw new ArgumentNullException(nameof(condition));

            var item = list.LastOrDefault(condition);
            if (Equals(item, null))
                return -1;

            return list.IndexOf(item);
        }

        public static List<List<T>> Split<T>(this List<T> list, int amounts)
        {
            var lists = new List<List<T>>();
            var index = 0;
            while (index < list.Count)
            {
                var count = amounts;
                if (list.Count - index <= amounts)
                    count = list.Count - index;

                lists.Add(list.GetRange(index, count));
                index += amounts;
            }

            return lists;
        }

        public static List<T> Shuffle<T>(this List<T> list)
        {
            return list.OrderBy(x => Guid.NewGuid()).ToList();
        }

        public static IList<T> Shuffle<T>(this IList<T> list)
        {
            return list.OrderBy(x => Guid.NewGuid()).ToList();
        }
    }
}