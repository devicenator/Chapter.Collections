// 
// Copyright (c) David Wendland. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 

using System;

// ReSharper disable once CheckNamespace

namespace SniffCore.Collections;

/// <summary>
///     Invokes actions directly.
/// </summary>
public sealed class DirectInvokator : IInvokator
{
    /// <summary>
    ///     Invokes an action.
    /// </summary>
    /// <param name="action">The action to invoke.</param>
    public void Invoke(Action action)
    {
        action();
    }
}