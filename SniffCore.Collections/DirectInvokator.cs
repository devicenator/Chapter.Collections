//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;

namespace SniffCore.Collections
{
    public sealed class DirectInvokator : IInvokator
    {
        public void Invoke(Action action)
        {
            action();
        }
    }
}