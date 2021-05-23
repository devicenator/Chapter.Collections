//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;
using System.Collections.Generic;
using System.Linq;

namespace SniffCore.Collections
{
    public static class EnumerableEx
    {
        public static IEnumerable<TResult> Repeat<TResult>(Func<TResult> elementCallback, int count)
        {
            if (elementCallback == null)
                throw new ArgumentNullException(nameof(elementCallback));

            for (var i = 0; i < count; ++i)
                yield return elementCallback();
        }

        public static void ForEach<T>(this IEnumerable<T> elements, Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            foreach (var element in elements)
                action(element);
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> elements)
        {
            return elements.OrderBy(x => Guid.NewGuid());
        }
    }
}