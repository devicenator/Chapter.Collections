//
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
//

using System;

// ReSharper disable once CheckNamespace

namespace SniffCore.Collections
{
    internal class DisableNotifications : IDisposable
    {
        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Disposed;
    }
}